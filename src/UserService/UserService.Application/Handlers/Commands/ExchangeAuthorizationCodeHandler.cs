using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.EntityFrameworkCore.Models;
using System.Security.Claims;
using UserService.Application.InternalCommands;

using static OpenIddict.Abstractions.OpenIddictConstants;

namespace UserService.Application.Handlers.Commands;

internal class ExchangeAuthorizationCodeHandler(IOpenIddictApplicationManager applicationManager, IOpenIddictScopeManager scopeManager)
    : IRequestHandler<ExchangeAuthorizationCode, Result<ClaimsPrincipal, string>>
{
    public async Task<Result<ClaimsPrincipal, string>> Handle(ExchangeAuthorizationCode command, CancellationToken cancellationToken)
    {
        var request = command.OpenIddictRequest;

        // Validate the client application.
        var applicationObj = await applicationManager.FindByClientIdAsync(request.ClientId!, cancellationToken);
        if (applicationObj is not OpenIddictEntityFrameworkCoreApplication application)
        {
            return "Invalid client";
        }

        var identity = new ClaimsIdentity(
            TokenValidationParameters.DefaultAuthenticationType,
            Claims.Name,
            Claims.Role);

        var resources = await scopeManager
            .ListResourcesAsync(identity.GetScopes(), cancellationToken)
            .ToListAsync(cancellationToken);

        identity.SetClaim(Claims.Subject, application.ClientId)
                .SetClaim(Claims.Name, application.DisplayName)
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
