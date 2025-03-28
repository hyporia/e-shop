using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Application.Utils.Abstractions;
using ProductService.Data.Services;
using ProductService.Domain;

namespace ProductService.Data.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddData(this IServiceCollection services, string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        return services
            .AddDbContext<ProductDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            })
            .AddTransient<IQueries<Product>, Queries<Product>>()
            ;
    }
}