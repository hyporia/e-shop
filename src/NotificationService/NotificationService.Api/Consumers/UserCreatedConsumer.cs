using MassTransit;
using UserService.Events;

namespace NotificationService.Api.Consumers;

public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
{
    private readonly ILogger<UserCreatedConsumer> _logger;

    public UserCreatedConsumer(ILogger<UserCreatedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        _logger.LogInformation("User created: {@user}", context.Message);
        return Task.CompletedTask;
    }
}
