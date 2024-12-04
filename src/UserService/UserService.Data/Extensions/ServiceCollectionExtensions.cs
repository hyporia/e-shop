using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Utils.Abstractions;
using UserService.Data.Services;
using UserService.Domain;

namespace UserService.Data.Extensions;
public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString) =>
		services
			.AddScoped<UserDbContext>(x => new(connectionString))
			//.AddHostedService<DbMigratorService>()
			.AddTransient<IQueries<User>, Queries<User>>()
			;
}