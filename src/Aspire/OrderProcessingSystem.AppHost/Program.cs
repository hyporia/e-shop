var builder = DistributedApplication.CreateBuilder(args);

var messaging = builder.AddRabbitMQ("messaging");

builder.AddProject<Projects.UserService_Api>("userservice")
    .WithReference(messaging);

builder.AddProject<Projects.NotificationService_Api>("notificationservice")
    .WithReference(messaging);

builder.Build().Run();
