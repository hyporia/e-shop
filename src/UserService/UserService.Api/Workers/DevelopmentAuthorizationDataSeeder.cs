using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace UserService.Api.Workers;

public class DevelopmentAuthorizationDataSeeder(IServiceScopeFactory serviceScopeFactory) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = serviceScopeFactory.CreateAsyncScope();
        await SeedDataAsync(scope, cancellationToken);
    }

    private static async Task SeedDataAsync(IServiceScope scope, CancellationToken cancellationToken)
    {
        await CreateApplicationsAsync();
        await CreateScopesAsync();

        async Task CreateApplicationsAsync()
        {
            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
            if (await manager.FindByClientIdAsync("swagger", cancellationToken) is not null)
                return;

            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var port = configuration["ASPNETCORE_HTTPS_PORT"];

            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = "swagger",
                ConsentType = ConsentTypes.Explicit,
                ClientType = ClientTypes.Public,
                RedirectUris =
                {
                    new Uri($"https://localhost:{port}/swagger/oauth2-redirect.html"),
                },
                Permissions =
                {
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.Logout,
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.AuthorizationCode,
                    Permissions.GrantTypes.RefreshToken,
                    Permissions.ResponseTypes.Code,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Roles,
                    Permissions.Prefixes.Scope + "user_api",
                },
                Requirements =
                {
                    Requirements.Features.ProofKeyForCodeExchange,
                },
            }, cancellationToken);
        }

        async Task CreateScopesAsync()
        {
            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();
            if (await manager.FindByNameAsync("user_api", cancellationToken) is not null)
                return;

            await manager.CreateAsync(new OpenIddictScopeDescriptor
            {
                Name = "user_api",
                // Resources = { "users_server" }
            }, cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
