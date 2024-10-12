using Microsoft.Extensions.DependencyInjection;
using UserService.Data.Services;

namespace UserService.Data.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString) =>
        services
            .AddScoped<UserDbContext>(x => new(connectionString))
            .AddHostedService<DbMigratorService>();
}