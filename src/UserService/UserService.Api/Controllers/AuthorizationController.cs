using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Net.Mime;
using System.Security.Claims;
using UserService.Domain;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace UserService.Api.Controllers;

[ApiController]
public class AuthorizationController(
    IHttpContextAccessor contextAccessor,
    IOpenIddictScopeManager openIddictScopeManager,
    IOpenIddictApplicationManager applicationManager,
    IOpenIddictScopeManager scopeManager,
    SignInManager<User> signInManager,
    UserManager<User> userManager) : ControllerBase
{
    private readonly HttpContext _context = contextAccessor.HttpContext!;
    private readonly IOpenIddictScopeManager _openIddictScopeManager = openIddictScopeManager;

    [HttpGet("~/connect/authorize")]
    [HttpPost("~/connect/authorize")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Create()
    {
        // Retrieve the OpenIddict server request from the HTTP context.
        var request = _context.GetOpenIddictServerRequest();

        ClaimsIdentity identity = new(
            TokenValidationParameters.DefaultAuthenticationType,
            Claims.Name,
            Claims.Role);

        identity.AddClaim(new Claim(Claims.Subject, request.ClientId!));

        identity.SetScopes(request.GetScopes());


        identity.SetResources(await _openIddictScopeManager.ListResourcesAsync(identity.GetScopes()).ToListAsync());

        // Allow all claims to be added in the access tokens.
        identity.SetDestinations(claim => [Destinations.AccessToken]);

        return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }


    [HttpPost("~/connect/token")]
    [IgnoreAntiforgeryToken]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> Exchange()
    {
        var request = HttpContext.GetOpenIddictServerRequest();

        return request?.GrantType switch
        {
            GrantTypes.AuthorizationCode => await ExchangeAuthorizationCode(request),
            GrantTypes.Password => await ExchangePassword(request),
            _ => Forbid("The specified grant type is not supported.")
        };
    }

    private async Task<IActionResult> ExchangePassword(OpenIddictRequest request)
    {
        var user = await userManager.FindByNameAsync(request.Username!);
        if (user == null || string.IsNullOrWhiteSpace(request.Password))
        {
            var properties = new AuthenticationProperties(new Dictionary<string, string?>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                    "The username/password couple is invalid."
            });

            return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, true);
        if (!result.Succeeded)
        {
            var properties = new AuthenticationProperties(new Dictionary<string, string?>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The username/password couple is invalid."
            });

            return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        // Create the claims-based identity that will be used by OpenIddict to generate tokens.
        var identity = new ClaimsIdentity(
            TokenValidationParameters.DefaultAuthenticationType,
            Claims.Name,
            Claims.Role);

        var roles = await userManager.GetRolesAsync(user);
        // Add the claims that will be persisted in the tokens.
        identity.SetClaim(Claims.Subject, user.Id)
            .SetClaim(Claims.Email, user.Email)
            .SetClaim(Claims.Name, user.UserName)
            .SetClaims(Claims.Role, [.. roles]);

        // Set the list of scopes granted to the client application. 

        identity.SetScopes(roles.SelectMany(GetScopesByRole).Distinct());

        identity.SetDestinations(GetDestinations);

        return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    private async Task<IActionResult> ExchangeAuthorizationCode(OpenIddictRequest request)
    {
        if (!request.IsAuthorizationCodeGrantType())
        {
            return Forbid("The specified grant type is not supported.");
        }

        // Note: the client credentials are automatically validated by OpenIddict:
        // if client_id or client_secret are invalid, this action won't be invoked.

        var application = await applicationManager.FindByClientIdAsync(request.ClientId!);
        if (application == null)
        {
            return Forbid(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidClient
                }));
        }

        // Create the claims-based identity that will be used by OpenIddict to generate tokens.
        var identity = new ClaimsIdentity(
            TokenValidationParameters.DefaultAuthenticationType,
            Claims.Name,
            Claims.Role);

        // Add the claims that will be persisted in the tokens (use the client_id as the subject identifier).
        identity.SetClaim(Claims.Subject, await applicationManager.GetClientIdAsync(application));
        identity.SetClaim(Claims.Name, await applicationManager.GetDisplayNameAsync(application));

        // Note: In the original OAuth 2.0 specification, the client credentials grant
        // doesn't return an identity token, which is an OpenID Connect concept.
        //
        // As a non-standardized extension, OpenIddict allows returning an id_token
        // to convey information about the client application when the "openid" scope
        // is granted (i.e specified when calling principal.SetScopes()). When the "openid"
        // scope is not explicitly set, no identity token is returned to the client application.

        // Set the list of scopes granted to the client application in access_token.
        identity.SetScopes(request.GetScopes());
        identity.SetResources(await scopeManager.ListResourcesAsync(identity.GetScopes()).ToListAsync());
        identity.SetDestinations(GetDestinations);

        return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    private static IEnumerable<string> GetDestinations(Claim claim)
    {
        switch (claim.Type)
        {
            case Claims.Name or Claims.PreferredUsername:
                yield return Destinations.AccessToken;

                if (claim.Subject!.HasScope(Scopes.Profile))
                {
                    yield return Destinations.IdentityToken;
                }

                yield break;

            case Claims.Email or Claims.Role:
                yield return Destinations.AccessToken;
                yield break;

            // it's a secret value
            case "AspNet.Identity.SecurityStamp": yield break;

            default:
                yield return Destinations.AccessToken;
                yield break;
        }
    }

    private static IEnumerable<string> GetScopesByRole(string role) => role switch
    {
        "admin" => ["user_api"],
        "user" => [Scopes.OpenId, Scopes.Profile, Scopes.Roles, Scopes.Email, Scopes.Phone],
        _ => Array.Empty<string>()
    };
}