using EShop.ServiceDefaults;
using ProductService.Data;
using ProductService.Data.Extensions;
using ProductService.Migrator;
using Shared.Data.Migrator;

var builder = Host.CreateApplicationBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("productDb")!;

builder.AddServiceDefaults();
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(DbMigrator<ProductDbContext>.ActivitySourceName));
builder.Services.AddLogging();
builder.Services.AddData(connectionString);
builder.Services.AddHostedService<DbMigrator<ProductDbContext>>();
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddTransient<IDataSeeder<ProductDbContext>, DataSeeder>();
}

var host = builder.Build();
host.Run();