using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using System.Security.Claims;
using UserService.Application.InternalCommands;
using UserService.Domain;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace UserService.Application.Handlers.Commands;

internal class ExchangeAuthorizationCodeHandler(IOpenIddictScopeManager scopeManager, UserManager<User> userManager,
    SignInManager<User> signInManager)
    : IRequestHandler<ExchangeAuthorizationCode, Result<ClaimsPrincipal, string>>
{
    public async Task<Result<ClaimsPrincipal, string>> Handle(ExchangeAuthorizationCode command, CancellationToken cancellationToken)
    {
        var request = command.OpenIddictRequest;

        // Retrieve the user profile corresponding to the authorization code/refresh token.
        var user = await userManager.FindByIdAsync(command.AuthenticateResult.Principal.GetClaim(Claims.Subject));
        if (user is null)
        {
            return "The token is no longer valid.";
        }

        // Ensure the user is still allowed to sign in.
        if (!await signInManager.CanSignInAsync(user))
        {
            return "The user is no longer allowed to sign in.";
        }

        var identity = new ClaimsIdentity(
            TokenValidationParameters.DefaultAuthenticationType,
            Claims.Name,
            Claims.Role);

        var resources = await scopeManager
            .ListResourcesAsync(identity.GetScopes(), cancellationToken)
            .ToListAsync(cancellationToken);

        identity.SetClaim(Claims.Subject, await userManager.GetUserIdAsync(user))
                .SetClaim(Claims.Email, await userManager.GetEmailAsync(user))
                .SetClaim(Claims.Name, await userManager.GetUserNameAsync(user))
                .SetClaims(Claims.Role, [.. (await userManager.GetRolesAsync(user))])
                .SetScopes(request.GetScopes())
                .SetResources(resources)
                .SetDestinations(GetDestinations);

        return new ClaimsPrincipal(identity);
    }

    private static IEnumerable<string> GetDestinations(Claim claim)
        => claim.Type switch
        {
            "AspNet.Identity.SecurityStamp" => [], // Exclude sensitive claims.
            _ => [Destinations.AccessToken]
        };
}