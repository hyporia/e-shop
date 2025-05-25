using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OrderService.Data;

internal class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<OrderDbContext>
{
    public OrderDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<OrderDbContext>()
            .UseNpgsql(".")
            .Options;
        return new(options);
    }
}
