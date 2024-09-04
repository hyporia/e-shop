using NotificationService.Api;

var builder = WebApplication.CreateBuilder(args);

builder.AddRabbitMQClient("messaging");

builder.Services.AddHostedService<MessageQueueSubscriber>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
