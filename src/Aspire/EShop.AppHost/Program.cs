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
        x.WithUrlForEndpoint("http", u => u.DisplayText = "pgAdmin");
    })
    .WithContainerName("aspire-e-shop-postgres")
    .WithLifetime(ContainerLifetime.Persistent);

var productDb = db.AddDatabase("productDb");
var userDb = db.AddDatabase("userDb");
var orderDb = db.AddDatabase("orderDb");

var userDbMigrator = builder.AddProject<Projects.UserService_Migrator>("userservice-dbmigrator")
    .WithReference(userDb)
    .WaitFor(userDb);

var userServiceApi = builder.AddProject<Projects.UserService_Api>("userservice-api")
    .WithReference(messaging)
    .WithReference(userDb)
    .WithUrlForEndpoint("https", u => u.DisplayText = "Scalar")
    .WaitFor(messaging)
    .WaitForCompletion(userDbMigrator);

// builder.AddProject<Projects.NotificationService_Api>("notificationservice")
//     .WithReference(messaging);

var productServiceMigrator = builder.AddProject<Projects.ProductService_Migrator>("productservice-migrator")
    .WithReference(productDb)
    .WaitFor(productDb);

var productServiceApi = builder.AddProject<Projects.ProductService_Api>("productservice-api")
    .WithReference(productDb)
    .WithUrlForEndpoint("https", u => u.DisplayText = "Scalar")
    .WaitForCompletion(productServiceMigrator);


var orderServiceMigrator = builder.AddProject<Projects.OrderService_Migrator>("orderservice-dbmigrator")
    .WithReference(orderDb)
    .WaitFor(orderDb);

var orderServiceApi = builder.AddProject<Projects.OrderService_Api>("orderservice-api")
    .WithReference(orderDb)
    .WithUrlForEndpoint("https", u => u.DisplayText = "Scalar")
    .WithEnvironment("Authentication__Issuer", userServiceApi.GetEndpoint("https"))
    .WaitForCompletion(orderServiceMigrator);

// builder.AddProject<Projects.ShippingService_Api>("shippingservice-api");

builder.AddNpmApp("react", "../../Clients/onlineshop", "dev")
    .WithReference(userServiceApi)
    .WithEnvironment("BROWSER", "none") // Disable opening browser on npm start
    .WithHttpEndpoint(3000, env: "VITE_PORT", isProxied: false)
    .WithUrlForEndpoint("http", u => u.DisplayText = "e-shop")
    .WithEnvironment("VITE_USERSERVICE_API_URL", userServiceApi.GetEndpoint("https"))
    .WithEnvironment("VITE_PRODUCTSERVICE_API_URL", productServiceApi.GetEndpoint("https"))
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();