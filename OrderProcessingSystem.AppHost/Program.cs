var builder = DistributedApplication.CreateBuilder(args);

var messaging = builder.AddRabbitMQ("messaging");

builder.AddProject<Projects.UserService>("userservice")
    .WithReference(messaging);

builder.AddProject<Projects.NotificationService>("emailservice")
    .WithReference(messaging);

builder.Build().Run();
