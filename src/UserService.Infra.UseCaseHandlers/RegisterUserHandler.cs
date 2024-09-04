using MediatR;
using Shared.Infra.CQRS;
using UserService.UseCases;

namespace UserService.Infra.UseCaseHandlers;

internal class RegisterUserHandler : IRequestHandler<RegisterUser, EmptyCommandResponse>
{
    public async Task<EmptyCommandResponse> Handle(RegisterUser request, CancellationToken cancellationToken)
    {
        return new();
    }
}