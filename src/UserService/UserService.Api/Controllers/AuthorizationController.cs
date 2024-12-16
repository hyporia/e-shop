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
    IOpenIddictScopeManager scopeManager,
    IOpenIddictApplicationManager applicationManager,
    SignInManager<User> signInManager,
    UserManager<User> userManager) : ControllerBase
{
    [HttpGet("~/connect/authorize")]
    [HttpPost("~/connect/authorize")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Authorize()
    {
        // Retrieve the OpenIddict server request from the HTTP context.
        var request = HttpContext.GetOpenIddictServerRequest();

        // Create a new identity with the provided authentication type.
        var identity = new ClaimsIdentity(
            TokenValidationParameters.DefaultAuthenticationType,
            Claims.Name,
            Claims.Role);

        // Add the client_id as the subject claim.
        identity.AddClaim(new Claim(Claims.Subject, request.ClientId!));

        // Set the scopes requested by the client application.
        identity.SetScopes(request.GetScopes());

        // Set the resources based on the scopes.
        var resources = await scopeManager
            .ListResourcesAsync(identity.GetScopes())
            .ToListAsync();

        identity.SetResources(resources);

        // Allow all claims to be included in the access token.
        identity.SetDestinations(_ => [Destinations.AccessToken]);

        // Return a sign-in result with the generated principal.
        return SignIn(
            principal: new ClaimsPrincipal(identity),
            authenticationScheme: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    // Another way is to return a confirmation view
    [HttpGet("~/connect/logout")]
    public async Task<IActionResult> Logout([FromQuery(Name = "post_logout_redirect_uri")] string? redirectUri)
        => await DoLogoutAsync(redirectUri);

    [HttpPost("~/connect/logout"), ValidateAntiForgeryToken]
    public async Task<IActionResult> LogoutPost([FromForm] string? postLogoutRedirectUri)
        => await DoLogoutAsync(postLogoutRedirectUri);

    private async Task<SignOutResult> DoLogoutAsync(string? redirectUri)
    {
        if (string.IsNullOrEmpty(redirectUri) || !Uri.IsWellFormedUriString(redirectUri, UriKind.Absolute))
        {
            redirectUri = "/";
        }

        // Ask ASP.NET Core Identity to delete the local and external cookies created
        // when the user agent is redirected from the external identity provider
        // after a successful authentication flow (e.g Google or Facebook).
        await signInManager.SignOutAsync();

        // Returning a SignOutResult will ask OpenIddict to redirect the user agent
        // to the post_logout_redirect_uri specified by the client application or to
        // the RedirectUri specified in the authentication properties if none was set.
        return SignOut(
            authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
            properties: new() { RedirectUri = redirectUri });
    }

    [HttpPost("~/connect/token")]
    [IgnoreAntiforgeryToken]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> Exchange(CancellationToken cancellationToken)
    {
        // Retrieve the OpenIddict server request.
        var request = HttpContext.GetOpenIddictServerRequest();

        return request?.GrantType switch
        {
            GrantTypes.AuthorizationCode => await ExchangeAuthorizationCodeAsync(request, cancellationToken),
            GrantTypes.Password => await ExchangePasswordAsync(request),
            _ => ForbidWithError("The specified grant type is not supported.")
        };
    }



    private async Task<IActionResult> ExchangePasswordAsync(OpenIddictRequest request)
    {
        // Validate the user credentials.
        var user = await userManager.FindByNameAsync(request.Username!);
        if (user == null || string.IsNullOrWhiteSpace(request.Password))
        {
            return ForbidWithError("The username/password combination is invalid.");
        }

        var signInResult = await signInManager.CheckPasswordSignInAsync(
            user,
            request.Password,
            lockoutOnFailure: true);

        if (!signInResult.Succeeded)
        {
            return ForbidWithError("Invalid credentials.");
        }

        // Create the claims-based identity.
        var identity = new ClaimsIdentity(
            TokenValidationParameters.DefaultAuthenticationType,
            Claims.Name,
            Claims.Role);

        // Add user claims.
        identity.SetClaim(Claims.Subject, user.Id)
                .SetClaim(Claims.Email, user.Email)
                .SetClaim(Claims.Name, user.UserName);

        var roles = await userManager.GetRolesAsync(user);
        identity.SetClaims(Claims.Role, [.. roles]);

        // Set scopes based on user roles.
        var scopes = roles.SelectMany(GetScopesByRole).Distinct();
        identity.SetScopes(scopes);

        // Set claim destinations.
        identity.SetDestinations(GetDestinations);

        // Return a sign-in result with the generated principal.
        return SignIn(
            principal: new ClaimsPrincipal(identity),
            authenticationScheme: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    private async Task<IActionResult> ExchangeAuthorizationCodeAsync(OpenIddictRequest request, CancellationToken cancellationToken)
    {
        if (!request.IsAuthorizationCodeGrantType())
        {
            return ForbidWithError("The specified grant type is not supported.");
        }

        // Validate the client application.
        var application = await applicationManager.FindByClientIdAsync(request.ClientId!, cancellationToken);
        if (application == null)
        {
            return ForbidWithError("Invalid client.");
        }

        // Create the claims-based identity.
        var identity = new ClaimsIdentity(
            TokenValidationParameters.DefaultAuthenticationType,
            Claims.Name,
            Claims.Role);

        // Add application claims.
        identity.SetClaim(Claims.Subject, await applicationManager.GetClientIdAsync(application, cancellationToken))
                .SetClaim(Claims.Name, await applicationManager.GetDisplayNameAsync(application, cancellationToken));

        // Set scopes and resources.
        identity.SetScopes(request.GetScopes());

        var resources = await scopeManager
            .ListResourcesAsync(identity.GetScopes(), cancellationToken)
            .ToListAsync(cancellationToken);

        identity.SetResources(resources);

        // Set claim destinations.
        identity.SetDestinations(GetDestinations);

        // Return a sign-in result with the generated principal.
        return SignIn(
            principal: new ClaimsPrincipal(identity),
            authenticationScheme: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    private ForbidResult ForbidWithError(string errorDescription)
    {
        var properties = new AuthenticationProperties(new Dictionary<string, string?>
        {
            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = errorDescription
        });

        return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    private static IEnumerable<string> GetDestinations(Claim claim) => claim.Type switch
    {
        Claims.Name or Claims.PreferredUsername => GetNameDestinations(claim),
        Claims.Email or Claims.Role => [Destinations.AccessToken],
        "AspNet.Identity.SecurityStamp" => [], // Exclude sensitive claims.
        _ => [Destinations.AccessToken]
    };

    private static IEnumerable<string> GetNameDestinations(Claim claim)
    {
        yield return Destinations.AccessToken;

        if (claim.Subject!.HasScope(Scopes.Profile))
        {
            yield return Destinations.IdentityToken;
        }
    }

    private static IEnumerable<string> GetScopesByRole(string role) => role switch
    {
        "admin" => ["user_api"],
        "user" => [Scopes.OpenId, Scopes.Profile, Scopes.Roles, Scopes.Email, Scopes.Phone],
        _ => Array.Empty<string>()
    };
}
