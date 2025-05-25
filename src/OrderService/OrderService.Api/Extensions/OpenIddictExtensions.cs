using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Validation.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace OrderService.Api.Extensions;

/// <summary>
/// Extensions for configuring OpenIddict validation.
/// </summary>
public static class OpenIddictExtensions
{
    /// <summary>
    /// Adds OpenIddict validation services to validate JWT tokens issued by UserService.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddOpenIddictValidation(this IServiceCollection services, IConfiguration? configuration = null)
    {
        // Default issuer URL if not provided in configuration
        var issuerUrl = configuration?.GetValue<string>("Authentication:Issuer")
            ?? throw new InvalidOperationException("Authentication:Issuer configuration is required for OpenIddict validation.");

        services.AddOpenIddict()
            .AddValidation(options =>
            {
                // Note: the validation handler uses OpenID Connect discovery
                // to retrieve the address of the introspection endpoint.
                options.SetIssuer(issuerUrl);
                options.AddAudiences("order_service");

                // Configure the validation handler to use introspection and register the client
                // credentials used when communicating with the remote introspection endpoint.
                options.UseIntrospection()
                       .SetClientId("order_service")
                       .SetClientSecret("ORDER-SERVICE-SECRET-KEY");

                // Register the System.Net.Http integration.
                options.UseSystemNetHttp();

                // Register the ASP.NET Core host.
                options.UseAspNetCore();
            });

        return services;
    }
}
