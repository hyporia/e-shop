using MassTransit;
using MediatR;
using Shared.Infra.CQRS;
using UserService.Events;
using UserService.UseCases;

namespace UserService.Handlers;

internal class RegisterUserHandler : IRequestHandler<RegisterUser, EmptyCommandResponse>
{
    private readonly IBus _bus;

    public RegisterUserHandler(IBus bus)
    {
        _bus = bus;
    }

    public async Task<EmptyCommandResponse> Handle(RegisterUser request, CancellationToken cancellationToken)
    {
        await _bus.Publish(new UserCreatedEvent(request.Email), cancellationToken);
        return new();
    }
}