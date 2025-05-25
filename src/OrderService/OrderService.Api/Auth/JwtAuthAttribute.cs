using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using OpenIddict.Validation.AspNetCore;

namespace OrderService.Api.Auth;

/// <summary>
/// Attribute to enforce JWT token authentication on FastEndpoints.
/// </summary>
public class JwtAuthAttribute : AuthorizeAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="JwtAuthAttribute"/> class.
    /// </summary>
    /// <param name="policy">The optional authorization policy to use.</param>
    public JwtAuthAttribute(string? policy = null)
    {
        // Set the authentication scheme to OpenIddict validation
        AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;

        if (!string.IsNullOrEmpty(policy))
        {
            Policy = policy;
        }
    }
}

/// <summary>
/// Attribute for allowing endpoints to be accessed without authentication.
/// Useful for specific public endpoints.
/// </summary>
public class PublicEndpointAttribute : AllowAnonymousAttribute
{
}
