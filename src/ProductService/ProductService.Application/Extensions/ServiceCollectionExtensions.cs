using Microsoft.Extensions.DependencyInjection;
using ProductService.Application.Handlers.Queries;

namespace ProductService.Application.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
        => services
            .AddMediatR(x => x.RegisterServicesFromAssemblyContaining<GetProductsHandler>())
            ;
}