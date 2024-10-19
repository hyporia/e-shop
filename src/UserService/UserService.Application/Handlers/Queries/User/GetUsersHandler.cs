
namespace UserService.Application.Handlers.Queries.User;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using UserService.Application.Utils.Abstractions;
using UserService.Contracts.Queries.User;
using UserService.Domain;

internal class GetUsersHandler(IQueries<User> userQueries) : IRequestHandler<GetUsers, GetUsersResponse>
{
    public async Task<GetUsersResponse> Handle(GetUsers request, CancellationToken cancellationToken)
    {
        var users = await userQueries.GetAllAsync(cancellationToken);
        return new()
        {
            Users = users.Select(user => new GetUsersResponseUser
            {
                Id = user.Id,
                Name = user.UserName!
            }).ToList()
        };
    }
}