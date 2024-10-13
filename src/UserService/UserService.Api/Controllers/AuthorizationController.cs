using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace UserService.Api.Controllers;

[ApiController]
public class AuthorizationController(
    IHttpContextAccessor contextAccessor,
    IOpenIddictScopeManager openIddictScopeManager) : ControllerBase
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

        var identity = new ClaimsIdentity(
            authenticationType: TokenValidationParameters.DefaultAuthenticationType,
            nameType: Claims.Name,
            roleType: Claims.Role);

        identity.AddClaim(new Claim(Claims.Subject, request.ClientId!));

        identity.SetScopes(request.GetScopes());


        identity.SetResources(await _openIddictScopeManager.ListResourcesAsync(identity.GetScopes()).ToListAsync());

        // Allow all claims to be added in the access tokens.
        identity.SetDestinations(claim => [Destinations.AccessToken]);

        return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}