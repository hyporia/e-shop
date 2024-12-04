using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Handlers.Queries.User;

namespace UserService.Application.Extensions;
public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
		=> services
			.AddMediatR(x => x.RegisterServicesFromAssemblyContaining<GetUsersHandler>())
			;
}