using Shared.Infra.CQRS;

namespace UserService.UseCases;

public class RegisterUser : Command
{
    public required string Email { get; init; }

    public required string Password { get; init; }
}