using EShop.ServiceDefaults;
using Shared.Data.Migrator;
using OrderService.Data;
using OrderService.Data.Extensions;

var builder = Host.CreateApplicationBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("orderDb")!;

builder.AddServiceDefaults();
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(DbMigrator<OrderDbContext>.ActivitySourceName));
builder.Services.AddLogging();
builder.Services.AddData(connectionString);
builder.Services.AddHostedService<DbMigrator<OrderDbContext>>();

var host = builder.Build();
host.Run();