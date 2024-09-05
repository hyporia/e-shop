using MassTransit;
using System.Reflection;
using UserService.Handlers.Extensions;
var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddUseCaseHandlers();
builder.Services.AddMassTransit(x =>
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
var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
