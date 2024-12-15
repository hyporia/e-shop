using OrderProcessingSystem.ServiceDefaults;
using UserService.Data.Extensions;
using UserService.DbMigrator;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddLogging();
builder.Services.AddHostedService<Worker>();
builder.Services.AddDatabase(builder.Configuration.GetConnectionString("postgresql")!);

var host = builder.Build();
host.Run();
