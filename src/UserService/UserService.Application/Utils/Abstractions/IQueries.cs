namespace UserService.Application.Utils.Abstractions;

public interface IQueries<TEntity>
    where TEntity : class
{
    Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
}
