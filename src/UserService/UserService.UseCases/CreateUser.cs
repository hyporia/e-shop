namespace UserService.UseCases;

public record CreateUser
{
    public CreateUser(string name, string email)
    {
        Name = name;
        Email = email;
    }

    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
}