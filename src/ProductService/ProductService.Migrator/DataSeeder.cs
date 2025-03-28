using ProductService.Data;
using ProductService.Domain;
using Shared.Data.Migrator;

namespace ProductService.Migrator;

internal class DataSeeder : IDataSeeder<ProductDbContext>
{
    public IEnumerable<object> Entities =>
    [
        Product.Create("Apple iPhone 13", 799.99m, "Latest model of Apple iPhone with A15 Bionic chip").Value,
        Product.Create("Samsung Galaxy S21", 699.99m, "Samsung's flagship smartphone with Exynos 2100").Value,
        Product.Create("Sony WH-1000XM4", 349.99m, "Industry-leading noise canceling over-ear headphones").Value,
        Product.Create("Dell XPS 13", 999.99m, "High-performance laptop with Intel i7 processor").Value,
        Product.Create("Apple MacBook Pro", 1299.99m, "Apple's powerful laptop with M1 chip").Value,
        Product.Create("Google Pixel 6", 599.99m, "Google's latest smartphone with Tensor chip").Value,
        Product.Create("Bose QuietComfort 35 II", 299.99m, "Wireless Bluetooth headphones with noise cancellation").Value,
        Product.Create("Microsoft Surface Pro 7", 749.99m, "Versatile 2-in-1 laptop with Intel i5 processor").Value,
        Product.Create("Amazon Echo Dot", 49.99m, "Smart speaker with Alexa voice assistant").Value,
        Product.Create("Fitbit Charge 5", 179.99m, "Advanced fitness and health tracker").Value
    ];

    public bool ShouldSeed(ProductDbContext context) => !context.Products.Any();
}
