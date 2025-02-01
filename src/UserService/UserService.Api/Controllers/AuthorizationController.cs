using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Server.AspNetCore;
using System.Net.Mime;
using System.Security.Claims;
using UserService.Application.InternalCommands;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace UserService.Api.Controllers;

[ApiController]
public class AuthorizationController(IMediator mediator) : ControllerBase
{
    [HttpGet("~/connect/authorize")]
    [HttpPost("~/connect/authorize")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Authorize()
        => SignIn(
                principal: await mediator.Send(new IssueAuthorizationCode(HttpContext.GetOpenIddictServerRequest()!)),
                authenticationScheme: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme
            );

    // Another way is to return a confirmation view
    [HttpGet("~/connect/logout")]
    public async Task<IActionResult> Logout([FromQuery(Name = "post_logout_redirect_uri")] string? redirectUri)
        => await DoLogoutAsync(redirectUri);

    [HttpPost("~/connect/logout"), ValidateAntiForgeryToken]
    public async Task<IActionResult> LogoutPost([FromForm(Name = "post_logout_redirect_uri")] string? redirectUri)
        => await DoLogoutAsync(redirectUri);

    private async Task<SignOutResult> DoLogoutAsync(string? redirectUri)
        => SignOut(
            authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
            properties: new() { RedirectUri = await mediator.Send(new LogoutUser(redirectUri)) }
        );


    [HttpPost("~/connect/token")]
    [IgnoreAntiforgeryToken]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> Exchange(CancellationToken cancellationToken = default)
    {
        var request = HttpContext.GetOpenIddictServerRequest()!;

        Result<ClaimsPrincipal, string> result;
        AuthenticateResult? authenticateResult = null;
        switch (request?.GrantType)
        {
            case GrantTypes.AuthorizationCode:
                authenticateResult = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                result = await mediator.Send(new ExchangeAuthorizationCode(request, authenticateResult), cancellationToken);
                break;
            case GrantTypes.Password:
                result = await mediator.Send(new ExchangeUserCredentials(request), cancellationToken);
                break;
            case GrantTypes.RefreshToken:                
                authenticateResult = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                result = await mediator.Send(new ExchangeRefreshToken(request, authenticateResult), cancellationToken);
                break;
            default:
                result = Result.Failure<ClaimsPrincipal, string>("The specified grant type is not supported.");
                break;
        }

        return result.IsSuccess
            ? SignIn(result.Value, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)
            : CreateForbidResult(result.Error);
    }

    private static ForbidResult CreateForbidResult(string errorDescription)
    {
        var properties = new AuthenticationProperties(new Dictionary<string, string?>
        {
            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = errorDescription
        });

        return new(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, properties);
    }
}