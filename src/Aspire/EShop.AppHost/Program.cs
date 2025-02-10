var builder = DistributedApplication.CreateBuilder(args);

var messaging = builder
    .AddRabbitMQ("rabbitmq")
    .WithManagementPlugin()
    .WithLifetime(ContainerLifetime.Persistent);

var db = builder
    .AddPostgres("postgresql")
    .WithPgAdmin(x => x.WithLifetime(ContainerLifetime.Persistent))
    .WithLifetime(ContainerLifetime.Persistent);

var userDbMigrator = builder.AddProject<Projects.UserService_DbMigrator>("userservice-dbmigrator")
    .WithReference(db)
    .WaitFor(db);

var userServiceApi = builder.AddProject<Projects.UserService_Api>("userservice-api")
    .WithReference(messaging)
    .WithReference(db)
    .WaitFor(messaging)
    .WaitForCompletion(userDbMigrator);

var productDbMigrator = builder.AddProject<Projects.ProductService_Migrator>("productservice-dbmigrator")
    .WithReference(db)
    .WaitFor(db);

// builder.AddProject<Projects.NotificationService_Api>("notificationservice")
//     .WithReference(messaging);

// builder.AddProject<Projects.ProductService_Api>("productservice-api");

// builder.AddProject<Projects.OrderService_Api>("orderservice-api");

// builder.AddProject<Projects.ShippingService_Api>("shippingservice-api");

builder.AddNpmApp("react", "../../Clients/onlineshop", "dev")
    .WithReference(userServiceApi)
    .WithEnvironment("BROWSER", "none") // Disable opening browser on npm start
    .WithHttpEndpoint(3000, env: "VITE_PORT", isProxied: false)
    .WithEnvironment("VITE_USERSERVICE_API_URL", userServiceApi.GetEndpoint("https"))
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();