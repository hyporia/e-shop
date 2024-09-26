using OrderProcessingSystem.ServiceDefaults;
using UserService.Data;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<DbMigratorService>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(DbMigratorService.ActivitySourceName));

builder.AddNpgsqlDbContext<UserDbContext>("postgresql");

var host = builder.Build();
host.Run();
