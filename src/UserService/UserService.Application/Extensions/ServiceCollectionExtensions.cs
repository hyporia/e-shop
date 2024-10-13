using Microsoft.Extensions.DependencyInjection;

namespace UserService.Handlers.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUseCaseHandlers(this IServiceCollection services)
        => services;
}