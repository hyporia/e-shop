namespace UserService;

public interface IEventPublisher
{
    void Publish<T>(T @event);
}
