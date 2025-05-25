using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain;

namespace OrderService.Data.Configurations;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.HasKey(ci => ci.Id);

        builder.Property(ci => ci.Id)
            .ValueGeneratedNever();

        builder.Property(ci => ci.CartId)
            .IsRequired();

        builder.Property(ci => ci.ProductId)
            .IsRequired();

        builder.Property(ci => ci.Quantity)
            .IsRequired();

        builder.Property(ci => ci.Price)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(ci => ci.ProductName)
            .HasMaxLength(500)
            .IsRequired();

        builder.HasIndex(ci => new { ci.CartId, ci.ProductId })
            .IsUnique();

        builder.ToTable("CartItems");
    }
}
