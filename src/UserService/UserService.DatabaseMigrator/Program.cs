using OrderProcessingSystem.ServiceDefaults;
using UserService.Data;
using UserService.DatabaseMigrator;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

builder.AddNpgsqlDbContext<UserDbContext>("postgresql");

var host = builder.Build();
host.Run();
