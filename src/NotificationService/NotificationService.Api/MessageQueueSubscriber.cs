
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace EmailService;

internal class MessageQueueSubscriber : IHostedService
{
    private readonly IConnection _connection;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<MessageQueueSubscriber> _logger;
    private IModel? _channel;

    public MessageQueueSubscriber(IServiceScopeFactory serviceScopeFactory, ILogger<MessageQueueSubscriber> logger, IConnection connection)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _connection = connection;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: "user_created_queue",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation("Received message: {@message}", message);
            // Process the message
        };

        _channel.BasicConsume(queue: "user_created_queue",
                             autoAck: true,
                             consumer: consumer);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _channel?.Close();
        return Task.CompletedTask;
    }
}
