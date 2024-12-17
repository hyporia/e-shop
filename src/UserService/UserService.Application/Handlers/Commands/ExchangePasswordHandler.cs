using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.EntityFrameworkCore.Models;
using System.Security.Claims;
using UserService.Application.InternalCommands;
using UserService.Domain;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace UserService.Application.Handlers.Commands;

internal class ExchangePasswordHandler(UserManager<User> userManager, SignInManager<User> signInManager,
    IOpenIddictApplicationManager applicationManager)
    : IRequestHandler<ExchangePassword, Result<ClaimsPrincipal, string>>
{
    public async Task<Result<ClaimsPrincipal, string>> Handle(ExchangePassword command, CancellationToken cancellationToken)
    {
        var request = command.OpenIddictRequest;

        var user = await userManager.FindByNameAsync(request.Username!);

        if (user == null || string.IsNullOrWhiteSpace(request.Password))
        {
            return Result.Failure<ClaimsPrincipal, string>("The username/password combination is invalid.");
        }

        var applicationObj = await applicationManager.FindByClientIdAsync(request.ClientId!, cancellationToken);
        if (applicationObj is not OpenIddictEntityFrameworkCoreApplication application)
        {
            return Result.Failure<ClaimsPrincipal, string>("Invalid client.");
        }

        var signInResult = await signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);

        if (!signInResult.Succeeded)
        {
            return Result.Failure<ClaimsPrincipal, string>("Invalid credentials.");
        }

        var identity = await CreateIdentity(user, application);

        return Result.Success<ClaimsPrincipal, string>(new(identity));
    }

    private async Task<ClaimsIdentity> CreateIdentity(User user, OpenIddictEntityFrameworkCoreApplication application)
    {
        var identity = new ClaimsIdentity(
                   TokenValidationParameters.DefaultAuthenticationType,
                   Claims.Name,
                   Claims.Role);

        identity.SetClaim(Claims.Subject, user.Id)
                .SetClaim(Claims.Email, user.Email)
                .SetClaim(Claims.Name, user.UserName);

        var roles = await userManager.GetRolesAsync(user);
        identity.SetClaims(Claims.Role, [.. roles]);

        var scopes = roles.SelectMany(GetScopesByRole).Distinct();
        identity.SetScopes(scopes);

        identity.SetDestinations(claim => GetDestinations(claim, application));
        return identity;
    }

    private static IEnumerable<string> GetDestinations(Claim claim, OpenIddictEntityFrameworkCoreApplication? application = null)
    {
        if (ShouldGoToAccessToken(claim))
        {
            yield return Destinations.AccessToken;
        }

        if (ShouldGoToIdentityToken(claim, application))
        {
            yield return Destinations.IdentityToken;
        }
    }

    private static bool ShouldGoToAccessToken(Claim claim) =>
        claim.Type switch
        {
            "AspNet.Identity.SecurityStamp" => false, // Exclude sensitive claims.
            _ => true
        };

    private static bool ShouldGoToIdentityToken(Claim claim, OpenIddictEntityFrameworkCoreApplication? application)
        => claim.Type switch
        {
            // Only include the user's name in the identity token if the client application is allowed to access it.
            // TODO: and the applications actually requests it
            Claims.Name or Claims.PreferredUsername => application?.Permissions?.Contains(Permissions.Prefixes.Scope + Scopes.Profile) ?? false,
            _ => false
        };

    // TODO: move to DB?
    private static IEnumerable<string> GetScopesByRole(string role) => role switch
    {
        "admin" => ["user_api"],
        "user" => [Scopes.OpenId, Scopes.Profile, Scopes.Roles, Scopes.Email, Scopes.Phone],
        _ => Array.Empty<string>()
    };
}