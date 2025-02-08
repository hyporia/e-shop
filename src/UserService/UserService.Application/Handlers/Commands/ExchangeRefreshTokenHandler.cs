using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using System.Security.Claims;
using UserService.Application.InternalCommands;
using UserService.Domain;

namespace UserService.Application.Handlers.Commands;

internal class ExchangeRefreshTokenHandler(UserManager<User> userManager, SignInManager<User> signInManager, IOpenIddictScopeManager scopeManager) 
    : IRequestHandler<ExchangeRefreshToken, Result<ClaimsPrincipal, string>>
{
    public async Task<Result<ClaimsPrincipal, string>> Handle(ExchangeRefreshToken command, CancellationToken cancellationToken)
    {
        var request = command.OpenIddictRequest;

        // Retrieve the user profile corresponding to the authorization code/refresh token.
        var user = await userManager.FindByIdAsync(command.AuthenticateResult.Principal.GetClaim(OpenIddictConstants.Claims.Subject)!);
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
            OpenIddictConstants.Claims.Name,
            OpenIddictConstants.Claims.Role);

        var resources = await scopeManager
            .ListResourcesAsync(identity.GetScopes(), cancellationToken)
            .ToListAsync(cancellationToken);

        identity.SetClaim(OpenIddictConstants.Claims.Subject, await userManager.GetUserIdAsync(user))
            .SetClaim(OpenIddictConstants.Claims.Email, await userManager.GetEmailAsync(user))
            .SetClaim(OpenIddictConstants.Claims.Name, await userManager.GetUserNameAsync(user))
            .SetClaims(OpenIddictConstants.Claims.Role, [.. (await userManager.GetRolesAsync(user))])
            .SetScopes(request.GetScopes())
            .SetResources(resources)
            .SetDestinations(GetDestinations);

        return new ClaimsPrincipal(identity);
    }

    private static IEnumerable<string> GetDestinations(Claim claim)
        => claim.Type switch
        {
            "AspNet.Identity.SecurityStamp" => [], // Exclude sensitive claims.
            _ => [OpenIddictConstants.Destinations.AccessToken]
        };
}