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
        var request = HttpContext.GetOpenIddictServerRequest()!;

        // Create a new identity with the provided authentication type.
        var identity = new ClaimsIdentity(
            TokenValidationParameters.DefaultAuthenticationType,
            Claims.Name,
            Claims.Role);

        identity.AddClaim(new Claim(Claims.Subject, request.ClientId!));

        identity.SetScopes(request.GetScopes());

        // Set the resources based on the scopes.
        var resources = await scopeManager
            .ListResourcesAsync(identity.GetScopes())
            .ToListAsync();

        identity.SetResources(resources);

        identity.SetDestinations(_ => [Destinations.AccessToken]);

        return SignIn(
            principal: new ClaimsPrincipal(identity),
            authenticationScheme: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    // Another way is to return a confirmation view
    [HttpGet("~/connect/logout")]
    public async Task<IActionResult> Logout([FromQuery(Name = "post_logout_redirect_uri")] string? redirectUri)
        => await DoLogoutAsync(redirectUri);

    [HttpPost("~/connect/logout"), ValidateAntiForgeryToken]
    public async Task<IActionResult> LogoutPost([FromForm(Name = "post_logout_redirect_uri")] string? redirectUri)
        => await DoLogoutAsync(redirectUri);

    private async Task<SignOutResult> DoLogoutAsync(string? redirectUri)
    {
        await signInManager.SignOutAsync();

        if (string.IsNullOrEmpty(redirectUri) || !Uri.IsWellFormedUriString(redirectUri, UriKind.Absolute))
        {
            redirectUri = "/";
        }

        return SignOut(
            authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
            properties: new() { RedirectUri = redirectUri });
    }

    [HttpPost("~/connect/token")]
    [IgnoreAntiforgeryToken]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> Exchange(CancellationToken cancellationToken = default)
    {
        var request = HttpContext.GetOpenIddictServerRequest();

        return request?.GrantType switch
        {
            GrantTypes.AuthorizationCode => await ExchangeAuthorizationCodeAsync(request, cancellationToken),
            GrantTypes.Password => await ExchangePasswordAsync(request, cancellationToken),
            _ => ForbidWithError("The specified grant type is not supported.")
        };
    }

    private async Task<IActionResult> ExchangePasswordAsync(OpenIddictRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(request.Username!);
        if (user == null || string.IsNullOrWhiteSpace(request.Password))
        {
            return ForbidWithError("The username/password combination is invalid.");
        }

        var applicationObj = await applicationManager.FindByClientIdAsync(request.ClientId!, cancellationToken);
        if (applicationObj is not OpenIddictApplicationDescriptor application)
        {
            return ForbidWithError("Invalid client.");
        }

        var signInResult = await signInManager.CheckPasswordSignInAsync(
            user, request.Password, lockoutOnFailure: true);

        if (!signInResult.Succeeded)
        {
            return ForbidWithError("Invalid credentials.");
        }

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
        var applicationObj = await applicationManager.FindByClientIdAsync(request.ClientId!, cancellationToken);
        if (applicationObj is not OpenIddictApplicationDescriptor application)
        {
            return ForbidWithError("Invalid client.");
        }

        var identity = new ClaimsIdentity(
            TokenValidationParameters.DefaultAuthenticationType,
            Claims.Name,
            Claims.Role);

        identity.SetClaim(Claims.Subject, await applicationManager.GetClientIdAsync(application, cancellationToken))
                .SetClaim(Claims.Name, await applicationManager.GetDisplayNameAsync(application, cancellationToken));

        identity.SetScopes(request.GetScopes());

        var resources = await scopeManager
            .ListResourcesAsync(identity.GetScopes(), cancellationToken)
            .ToListAsync(cancellationToken);

        identity.SetResources(resources);

        identity.SetDestinations(claim => GetDestinations(claim));

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

    private static IEnumerable<string> GetDestinations(Claim claim, OpenIddictApplicationDescriptor? application = null)
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

    private static bool ShouldGoToIdentityToken(Claim claim, OpenIddictApplicationDescriptor? application)
        => claim.Type switch
        {
            // Only include the user's name in the identity token if the client application is allowed to access it.
            // TODO: and the applications actually requests it
            Claims.Name or Claims.PreferredUsername => application?.Permissions?.Contains(Scopes.Profile) ?? false,
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