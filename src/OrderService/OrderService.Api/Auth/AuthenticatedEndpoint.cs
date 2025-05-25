using FastEndpoints;
using OpenIddict.Validation.AspNetCore;
using System.Security.Claims;

namespace OrderService.Api.Auth;

/// <summary>
/// Base class for authenticated FastEndpoint endpoints.
/// </summary>
/// <typeparam name="TRequest">The request type.</typeparam>
/// <typeparam name="TResponse">The response type.</typeparam>
public abstract class AuthenticatedEndpoint<TRequest, TResponse> : Endpoint<TRequest, TResponse>
    where TRequest : notnull
{
    /// <summary>
    /// Configures the endpoint with authentication.
    /// </summary>
    public override void Configure()
    {
        ConfigureRoute();

        // Always require authentication using OpenIddict validation
        AuthSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);

        // Do not allow anonymous access
        DontThrowIfValidationFails();

        ConfigureDescription();
    }

    /// <summary>
    /// Configure the route for this endpoint.
    /// </summary>
    protected abstract void ConfigureRoute();

    /// <summary>
    /// Configure the description for this endpoint.
    /// </summary>
    protected virtual void ConfigureDescription() { }

    /// <summary>
    /// Get the current user's ID from claims.
    /// </summary>
    /// <returns>The user's ID as a Guid.</returns>
    protected Guid GetCurrentUserId()
    {
        // Try to extract the user ID from the "sub" claim
        var userIdClaim = User.FindFirstValue("sub") ??
                          User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new InvalidOperationException("Unable to determine current user ID from the token");
        }

        return userId;
    }
}
