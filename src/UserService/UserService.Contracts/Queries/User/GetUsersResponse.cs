namespace UserService.Contracts.Queries.User;

public class GetUsersResponse
{
    public List<GetUsersResponseUser> Users { get; set; } = [];
}

public class GetUsersResponseUser
{
    public required string Id { get; set; }
    public required string Name { get; set; }
}