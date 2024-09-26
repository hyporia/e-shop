var builder = DistributedApplication.CreateBuilder(args);

var messaging = builder.AddRabbitMQ("rabbitmq")
    .WithManagementPlugin();

var userDb = builder
    .AddPostgres("postgresql")
    .WithPgAdmin();

var userServiceApi = builder.AddProject<Projects.UserService_Api>("userservice")
    .WithReference(messaging)
    .WithReference(userDb);

builder.AddProject<Projects.NotificationService_Api>("notificationservice")
    .WithReference(messaging);

builder.AddProject<Projects.ProductService_Api>("productservice-api");

builder.AddProject<Projects.OrderService_Api>("orderservice-api");

builder.AddProject<Projects.ShippingService_Api>("shippingservice-api");

//builder.AddNpmApp("react", "../../Clients/onlineshop", scriptName: "dev")
//    .WithReference(userServiceApi)
//    .WithEnvironment("BROWSER", "none") // Disable opening browser on npm start
//    .WithHttpEndpoint(env: "PORT")
//    .WithExternalHttpEndpoints()
//    .PublishAsDockerFile();

builder.Build().Run();
