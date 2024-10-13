using Shared.Infra.CQRS;

namespace UserService.Application.InternalCommands;
public class RegisterUser : Command
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}