using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ProductService.Data;

public static class EntryPoint
{
    public static IServiceCollection AddData(this IServiceCollection services, string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        services.AddDbContext<ProductDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        return services;
    }
}