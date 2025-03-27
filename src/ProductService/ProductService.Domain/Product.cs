using CSharpFunctionalExtensions;

namespace ProductService.Domain;

public class Product : Entity<Guid>
{
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public string Description { get; private set; }

    private Product(string name, decimal price, string description)
    {
        Name = name;
        Price = price;
        Description = description;
    }

    public static Result<Product> Create(string name, decimal price, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<Product>("Name cannot be empty");
        }

        if (price <= 0)
        {
            return Result.Failure<Product>("Price must be greater than 0");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            return Result.Failure<Product>("Description cannot be empty");
        }

        return Result.Success<Product>(new(name, price, description));
    }
}