using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Utils.Abstractions;
using OrderService.Data.Repositories;

namespace OrderService.Data.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddData(this IServiceCollection services, string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        return services
            .AddDbContext<OrderDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            })
            .AddScoped<ICartRepository, CartRepository>();
    }
}
