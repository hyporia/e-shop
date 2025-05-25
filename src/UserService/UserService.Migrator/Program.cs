using EShop.ServiceDefaults;
using Shared.Data.Migrator;
using UserService.Data;
using UserService.Data.Extensions;

var builder = Host.CreateApplicationBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("userDb")!;

builder.AddServiceDefaults();
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(DbMigrator<UserDbContext>.ActivitySourceName));
builder.Services.AddLogging();
builder.Services.AddData(connectionString);
builder.Services.AddHostedService<DbMigrator<UserDbContext>>();

var host = builder.Build();
host.Run();