var builder = DistributedApplication.CreateBuilder(args);

var messaging = builder
    .AddRabbitMQ("rabbitmq")
    .WithManagementPlugin()
    .WithLifetime(ContainerLifetime.Persistent);

var userDb = builder
    .AddPostgres("postgresql")
    .WithPgAdmin(x => x.WithLifetime(ContainerLifetime.Persistent))
    .WithLifetime(ContainerLifetime.Persistent);

var userDbMigrator = builder.AddProject<Projects.UserService_DbMigrator>("userservice-dbmigrator")
    .WithReference(userDb)
    .WaitFor(userDb);

var userServiceApi = builder.AddProject<Projects.UserService_Api>("userservice-api")
    .WithReference(messaging)
    .WithReference(userDb)
    .WaitFor(messaging)
    .WaitForCompletion(userDbMigrator);

// builder.AddProject<Projects.NotificationService_Api>("notificationservice")
//     .WithReference(messaging);

// builder.AddProject<Projects.ProductService_Api>("productservice-api");

// builder.AddProject<Projects.OrderService_Api>("orderservice-api");

// builder.AddProject<Projects.ShippingService_Api>("shippingservice-api");

builder.AddNpmApp("react", "../../Clients/onlineshop", scriptName: "dev")
    .WithReference(userServiceApi)
    .WithEnvironment("BROWSER", "none") // Disable opening browser on npm start
    .WithHttpEndpoint(port: 3000, env: "VITE_PORT", isProxied: false)
    .WithEnvironment("VITE_USERSERVICE_API_URL", userServiceApi.GetEndpoint("https"))
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();