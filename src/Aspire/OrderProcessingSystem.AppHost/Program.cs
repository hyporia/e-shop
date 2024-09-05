var builder = DistributedApplication.CreateBuilder(args);

var messaging = builder.AddRabbitMQ("rabbitmq")
    .WithManagementPlugin();

builder.AddProject<Projects.UserService_Api>("userservice")
    .WithReference(messaging);

builder.AddProject<Projects.NotificationService_Api>("notificationservice")
    .WithReference(messaging);

builder.AddProject<Projects.ProductService_Api>("productservice-api");

builder.AddProject<Projects.OrderService_Api>("orderservice-api");

builder.AddProject<Projects.ShippingService_Api>("shippingservice-api");

builder.Build().Run();
