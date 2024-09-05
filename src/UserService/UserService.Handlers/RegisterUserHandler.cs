using MediatR;
using Shared.Infra.CQRS;
using UserService.UseCases;
using MassTransit;
using UserService.Events;

namespace UserService.Infra.UseCaseHandlers;

internal class RegisterUserHandler : IRequestHandler<RegisterUser, EmptyCommandResponse>
{
    private readonly IBus _bus;

    public RegisterUserHandler(IBus bus)
    {
        _bus = bus;
    }

    public async Task<EmptyCommandResponse> Handle(RegisterUser request, CancellationToken cancellationToken)
    {
        await _bus.Publish(new UserCreatedEvent(request.Name, request.Email), cancellationToken);
        return new();
    }
}