using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using UserService.Domain;
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
        await CreateRolesAsync(scope);
        await CreateCustomScopesAsync(scope, cancellationToken);
        await CreateApplicationsAsync(scope, cancellationToken);
        await CreateUsersAsync(scope);
    }

    private static async Task CreateRolesAsync(IServiceScope scope)
    {
        var manager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        foreach (var role in new[] { "admin", "user" })
        {
            if (await manager.FindByNameAsync(role) is not null)
            {
                continue;
            }

            await manager.CreateAsync(new IdentityRole(role));
        }
    }

    private static async Task CreateCustomScopesAsync(IServiceScope scope, CancellationToken cancellationToken)
    {
        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();
        if (await manager.FindByNameAsync("user_api", cancellationToken) is not null)
        {
            return;
        }

        await manager.CreateAsync(new OpenIddictScopeDescriptor
        {
            Name = "user_api"
            // Resources = { "users_server" }
        }, cancellationToken);
    }

    private static async Task CreateApplicationsAsync(IServiceScope scope, CancellationToken cancellationToken)
    {
        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var port = configuration["ASPNETCORE_HTTPS_PORT"];

        if (await manager.FindByClientIdAsync("scalar", cancellationToken) is null)
        {
            await manager.CreateAsync(
                new OpenIddictApplicationDescriptor
                {
                    ClientId = "scalar",
                    ConsentType = ConsentTypes.Explicit,
                    ClientType = ClientTypes.Public,
                    RedirectUris = { new Uri($"https://localhost:{port}/scalar/oauth2-redirect.html") },
                    Permissions =
                    {
                        Permissions.Endpoints.Authorization,
                        Permissions.Endpoints.Logout,
                        Permissions.Endpoints.Token,
                        Permissions.GrantTypes.AuthorizationCode,
                        Permissions.GrantTypes.RefreshToken,
                        Permissions.GrantTypes.Password,
                        Permissions.ResponseTypes.Code,
                        Permissions.Scopes.Email,
                        Permissions.Scopes.Profile,
                        Permissions.Scopes.Roles,
                        Permissions.Prefixes.Scope + "user_api"
                    },
                    Requirements = { Requirements.Features.ProofKeyForCodeExchange }
                }, cancellationToken
            );
        }

        if (await manager.FindByClientIdAsync("onlineshop", cancellationToken) is null)
        {
            await manager.CreateAsync(
                new OpenIddictApplicationDescriptor
                {
                    ClientId = "onlineshop",
                    ConsentType = ConsentTypes.Explicit,
                    ClientType = ClientTypes.Public,
                    RedirectUris = { new Uri($"https://localhost:3000/login/callback") },
                    PostLogoutRedirectUris = { new Uri("http://localhost:3000/home"), new Uri("http://localhost:3000") },
                    Permissions =
                    {
                        Permissions.Endpoints.Authorization,
                        Permissions.Endpoints.Logout,
                        Permissions.Endpoints.Token,
                        Permissions.GrantTypes.Password,
                        Permissions.GrantTypes.RefreshToken,
                        Permissions.ResponseTypes.IdTokenToken,
                        Permissions.Scopes.Email,
                        Permissions.Scopes.Profile,
                        Permissions.Scopes.Roles,
                        Permissions.Prefixes.Scope + "user_api"
                    },
                    Requirements = { Requirements.Features.ProofKeyForCodeExchange }
                }, cancellationToken
            );
        }
    }

    private static async Task CreateUsersAsync(IServiceScope scope)
    {
        var manager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        if (await manager.FindByEmailAsync("admin@localhost") is not null)
        {
            return;
        }

        var user = new User { Email = "admin@localhost", UserName = "admin" };

        await manager.CreateAsync(user, "123");
        await manager.AddToRolesAsync(user, ["admin", "user"]);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}