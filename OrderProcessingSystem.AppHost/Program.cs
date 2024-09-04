var builder = DistributedApplication.CreateBuilder(args);

var messaging = builder.AddRabbitMQ("messaging")
    .WithDockerfile("");

builder.AddProject<Projects.UserService>("userservice")
    .WithReference(messaging);

builder.AddProject<Projects.EmailService>("emailservice")
    .WithReference(messaging);

builder.Build().Run();
