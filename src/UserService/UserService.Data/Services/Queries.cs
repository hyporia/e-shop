using MassTransit.Internals;
using Microsoft.EntityFrameworkCore;
using UserService.Application.Utils.Abstractions;

namespace UserService.Data.Services;

public class Queries<TEntity>(UserDbContext dbContext) : IQueries<TEntity>
    where TEntity : class
{
    public Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        => dbContext.Set<TEntity>().AsNoTracking().ToListAsync(cancellationToken);
}
