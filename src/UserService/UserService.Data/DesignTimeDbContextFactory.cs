using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace UserService.Data;
internal class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
{
	public UserDbContext CreateDbContext(string[] args) => new(new DbContextOptionsBuilder<UserDbContext>().UseNpgsql(".").Options);
}