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

	private static async Task CreateUserAsync(IServiceScope scope)
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
		if (await manager.FindByClientIdAsync("swagger", cancellationToken) is not null)
		{
			return;
		}

		var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
		var port = configuration["ASPNETCORE_HTTPS_PORT"];

		await manager.CreateAsync(
			new OpenIddictApplicationDescriptor
			{
				ClientId = "swagger",
				ConsentType = ConsentTypes.Explicit,
				ClientType = ClientTypes.Public,
				RedirectUris = { new Uri($"https://localhost:{port}/swagger/oauth2-redirect.html") },
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
					Permissions.Prefixes.Scope + "user_api"
				},
				Requirements = { Requirements.Features.ProofKeyForCodeExchange }
			}, cancellationToken);

		await manager.CreateAsync(
			new OpenIddictApplicationDescriptor
			{
				ClientId = "onlineshop",
				ConsentType = ConsentTypes.Explicit,
				ClientType = ClientTypes.Public,
				RedirectUris = { new Uri($"https://localhost:3000/login/callback") },
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
			}, cancellationToken);
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