using Microsoft.EntityFrameworkCore;

namespace Shared.Data.Migrator;
public interface IDataSeeder<TContext>
    where TContext : DbContext
{
    IEnumerable<object> Entities { get; }

    bool ShouldSeed(TContext context);
}