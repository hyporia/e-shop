using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Net.Mime;
using System.Security.Claims;
using UserService.Application.InternalCommands;
using UserService.Domain;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace UserService.Api.Controllers;

[ApiController]
public class AuthorizationController(
    IOpenIddictScopeManager scopeManager,
    SignInManager<User> signInManager,
    IMediator mediator) : ControllerBase
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

        var result = request?.GrantType switch
        {
            GrantTypes.AuthorizationCode => await mediator.Send(new ExchangeAuthorizationCode(request), cancellationToken),
            GrantTypes.Password => await mediator.Send(new ExchangePassword(request), cancellationToken),
            _ => Result.Failure<ClaimsPrincipal, string>("The specified grant type is not supported.")
        };

        return result.IsSuccess
            ? SignIn(result.Value, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)
            : ForbidWithError(result.Error);
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
}