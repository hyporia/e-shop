using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using UserService.Domain;

namespace UserService.Data;

internal class UserDbContext : IdentityDbContext<User>
{
    private const string SchemaName = "id";
    private readonly string _connectionString;

    public UserDbContext(string connectionString)
    {
        _connectionString = new NpgsqlConnectionStringBuilder(connectionString).ConnectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseNpgsql(_connectionString, cfg => cfg.MigrationsHistoryTable("__EFMigrationsHistory", SchemaName));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(SchemaName);
    }
}
