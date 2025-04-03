var builder = DistributedApplication.CreateBuilder(args);

var messaging = builder
    .AddRabbitMQ("rabbitmq")
    .WithManagementPlugin()
    .WithContainerName("aspire-e-shop-rabbitmq")
    .WithLifetime(ContainerLifetime.Persistent);

var db = builder
    .AddPostgres("postgresql")
    .WithPgAdmin(x =>
    {
        x.WithLifetime(ContainerLifetime.Persistent);
        x.WithImageTag("9.1.0");
        x.WithContainerName("aspire-e-shop-pgadmin");
    })
    .WithContainerName("aspire-e-shop-postgres")
    .WithLifetime(ContainerLifetime.Persistent);

var productDb = db.AddDatabase("productDb");
var userDb = db.AddDatabase("userDb");

var userDbMigrator = builder.AddProject<Projects.UserService_DbMigrator>("userservice-dbmigrator")
    .WithReference(userDb)
    .WaitFor(userDb);

var userServiceApi = builder.AddProject<Projects.UserService_Api>("userservice-api")
    .WithReference(messaging)
    .WithReference(userDb)
    .WaitFor(messaging)
    .WaitForCompletion(userDbMigrator);

var productDbMigrator = builder.AddProject<Projects.ProductService_Migrator>("productservice-dbmigrator")
    .WithReference(productDb)
    .WaitFor(productDb);

// builder.AddProject<Projects.NotificationService_Api>("notificationservice")
//     .WithReference(messaging);

var productServiceApi = builder.AddProject<Projects.ProductService_Api>("productservice-api")
    .WithReference(productDb)
    .WaitForCompletion(productDbMigrator);
// builder.AddProject<Projects.OrderService_Api>("orderservice-api");

// builder.AddProject<Projects.ShippingService_Api>("shippingservice-api");

builder.AddNpmApp("react", "../../Clients/onlineshop", "dev")
    .WithReference(userServiceApi)
    .WithEnvironment("BROWSER", "none") // Disable opening browser on npm start
    .WithHttpEndpoint(3000, env: "VITE_PORT", isProxied: false)
    .WithEnvironment("VITE_USERSERVICE_API_URL", userServiceApi.GetEndpoint("https"))
    .WithEnvironment("VITE_PRODUCTSERVICE_API_URL", productServiceApi.GetEndpoint("https"))
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();