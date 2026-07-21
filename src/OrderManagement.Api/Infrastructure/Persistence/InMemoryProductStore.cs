using System.Collections.Concurrent;
using OrderManagement.Api.Domain.Products;

namespace OrderManagement.Api.Infrastructure.Persistence;

public sealed class InMemoryProductStore
{
    private readonly ConcurrentDictionary<Guid, Product> _products = new();

    public void Add(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        if (!_products.TryAdd(product.Id, product))
        {
            throw new InvalidOperationException(
                $"A product with ID '{product.Id}' already exists.");
        }
    }

    public Product? GetById(Guid id)
    {
        _products.TryGetValue(id, out var product);

        return product;
    }
    public bool ExistsBySku(string sku)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sku);

        var normalizedSku = sku.Trim().ToUpperInvariant();

        return _products.Values.Any(
            product => product.Sku == normalizedSku);
    }

    public IReadOnlyCollection<Product> GetAll()
    {
        return _products.Values.ToArray();
    }
}