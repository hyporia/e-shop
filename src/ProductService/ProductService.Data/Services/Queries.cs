using Microsoft.EntityFrameworkCore;
using ProductService.Application.Utils.Abstractions;

namespace ProductService.Data.Services;

internal class Queries<TEntity>(ProductDbContext dbContext) : IQueries<TEntity>
    where TEntity : class
{
    public Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        => dbContext.Set<TEntity>().AsNoTracking().ToListAsync(cancellationToken);
}
