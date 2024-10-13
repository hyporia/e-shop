using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserService.Domain;

namespace UserService.Data;

public class UserDbContext : IdentityDbContext<User>
{
    private const string SchemaName = "id";
    private readonly string _connectionString;

    public UserDbContext(string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder
            .UseOpenIddict()
            .UseNpgsql(_connectionString, cfg => cfg.MigrationsHistoryTable("__EFMigrationsHistory", SchemaName));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(SchemaName);
    }
}