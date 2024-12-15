using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserService.Domain;

namespace UserService.Data;

public class UserDbContext : IdentityDbContext<User>
{
	private const string SchemaName = "id";

	public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
		optionsBuilder
			.UseOpenIddict()
			.UseNpgsql(cfg => cfg.MigrationsHistoryTable("__EFMigrationsHistory", SchemaName));

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.HasDefaultSchema(SchemaName);
	}
}