using MassTransit;
using System.Reflection;

namespace UserService.Api.Extensions;

public static class MassTransitExtensions
{
    public static IServiceCollection AddMassTransit(this IServiceCollection services)
        => services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                x.SetInMemorySagaRepositoryProvider();

                var entryAssembly = Assembly.GetEntryAssembly();

                x.AddConsumers(entryAssembly);
                x.AddSagaStateMachines(entryAssembly);
                x.AddSagas(entryAssembly);
                x.AddActivities(entryAssembly);

                //builder.Services.Configure<MassTransitHostOptions>(options =>
                //{
                //    options.WaitUntilStarted = true;
                //});
                x.UsingRabbitMq((context, cfg) =>
                {
                    var configService = context.GetRequiredService<IConfiguration>();
                    var connectionString = configService.GetConnectionString("rabbitmq");
                    cfg.Host(connectionString);

                    cfg.ConfigureEndpoints(context);
                });
            });

}
