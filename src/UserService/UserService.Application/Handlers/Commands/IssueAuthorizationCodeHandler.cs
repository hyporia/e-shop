using MediatR;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using System.Security.Claims;
using UserService.Application.InternalCommands;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace UserService.Application.Handlers.Commands;

internal class IssueAuthorizationCodeHandler(IOpenIddictScopeManager scopeManager)
    : IRequestHandler<IssueAuthorizationCode, ClaimsPrincipal>
{
    public async Task<ClaimsPrincipal> Handle(IssueAuthorizationCode command, CancellationToken cancellationToken)
    {
        var request = command.OpenIddictRequest;

        var identity = new ClaimsIdentity(
            TokenValidationParameters.DefaultAuthenticationType,
            Claims.Name,
            Claims.Role);

        // Set the resources based on the scopes.
        var resources = await scopeManager
            .ListResourcesAsync(identity.GetScopes(), cancellationToken)
            .ToListAsync(cancellationToken);

        identity.SetClaim(Claims.Subject, request.ClientId!)
            .SetScopes(request.GetScopes())
            .SetResources(resources)
            .SetDestinations(_ => [Destinations.AccessToken]);

        return new(identity);
    }
}