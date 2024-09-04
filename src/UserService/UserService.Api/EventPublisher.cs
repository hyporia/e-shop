using RabbitMQ.Client;
using System.Text.Json;

namespace UserService;

internal class EventPublisher : IEventPublisher
{
    private readonly IConnection _connection;

    public EventPublisher(IConnection connection)
    {
        _connection = connection;
    }

    public void Publish<T>(T @event)
    {
        using var channel = _connection.CreateModel();
        channel.QueueDeclare(queue: "user_created_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

        var body = JsonSerializer.SerializeToUtf8Bytes(@event);

        channel.BasicPublish(exchange: "", routingKey: "user_created_queue", basicProperties: null, body: body);
    }
}