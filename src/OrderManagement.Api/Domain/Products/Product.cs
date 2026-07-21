using OrderManagement.Api.Domain.Common;

namespace OrderManagement.Api.Domain.Products;

public sealed class Product
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public string Sku { get; private set; }

    public decimal Price { get; private set; }

    public int Stock { get; private set; }

    public bool IsActive { get; private set; }

    private Product(
        Guid id,
        string name,
        string sku,
        decimal price,
        int stock,
        bool isActive)
    {
        Id = id;
        Name = name;
        Sku = sku;
        Price = price;
        Stock = stock;
        IsActive = isActive;
    }

    public static Product Create(
     string name,
     string sku,
     decimal price,
     int initialStock)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException(
                "Product name is required.");
        }

        if (string.IsNullOrWhiteSpace(sku))
        {
            throw new DomainException(
                "Product SKU is required.");
        }

        if (price <= 0)
        {
            throw new DomainException(
                "Product price must be greater than zero.");
        }

        if (initialStock < 0)
        {
            throw new DomainException(
                "Initial stock cannot be negative.");
        }

        return new Product(
            Guid.NewGuid(),
            name.Trim(),
            sku.Trim().ToUpperInvariant(),
            price,
            initialStock,
            true);
    }
}