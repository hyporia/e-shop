using Microsoft.Extensions.DependencyInjection;
using UserService.Infra.UseCaseHandlers;

namespace UserService.Handlers.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUseCaseHandlers(this IServiceCollection services)
        => services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining<RegisterUserHandler>());
}
