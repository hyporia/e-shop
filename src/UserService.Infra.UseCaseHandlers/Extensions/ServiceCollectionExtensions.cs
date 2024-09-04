using Microsoft.Extensions.DependencyInjection;

namespace UserService.Infra.UseCaseHandlers.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUseCaseHandlers(this IServiceCollection services)
        => services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining<RegisterUserHandler>());
}
