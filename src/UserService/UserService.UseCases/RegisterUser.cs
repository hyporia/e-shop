using Shared.Infra.CQRS;

namespace UserService.UseCases;

public class RegisterUser : Command
{
    public required string Name { get; set; }
    public required string Email { get; set; }
}
