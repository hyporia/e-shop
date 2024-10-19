using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Handlers.Queries.User;

namespace UserService.Handlers.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUseCaseHandlers(this IServiceCollection services)
        => services
            .AddMediatR(x => x.RegisterServicesFromAssemblyContaining<GetUsersHandler>())
            ;
}