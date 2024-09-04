namespace UserService.Controllers;

public class CreateUserRequest
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
}