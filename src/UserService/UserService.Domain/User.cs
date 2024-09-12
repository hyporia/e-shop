using Microsoft.AspNetCore.Identity;

namespace UserService.Domain;

public class User(string userName) : IdentityUser(userName)
{
}
