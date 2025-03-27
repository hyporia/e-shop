using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Utils.Abstractions;
using UserService.Data.Services;
using UserService.Domain;

namespace UserService.Data.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddData(this IServiceCollection services, string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        return services
            .AddDbContext<UserDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            })
            .AddTransient<IQueries<User>, Queries<User>>()
            ;
    }
}