using Microsoft.EntityFrameworkCore;
using ProductService.Data.EntityConfigurations;
using ProductService.Domain;

namespace ProductService.Data;

internal class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseNpgsql();

    protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);
}