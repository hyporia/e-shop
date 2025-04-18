using Microsoft.EntityFrameworkCore;
using ProductService.Application.Utils.Abstractions;

namespace ProductService.Data.Services;

internal class Queries<TEntity> : IQueries<TEntity>
    where TEntity : class
{
    private readonly ProductDbContext _dbContext;

    public Queries(ProductDbContext dbContext)
    {
        dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        _dbContext = dbContext;
    }

    public Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        => _dbContext.Set<TEntity>().AsNoTracking().ToListAsync(cancellationToken);

    public ValueTask<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => _dbContext.Set<TEntity>().FindAsync([id], cancellationToken: cancellationToken);
}
