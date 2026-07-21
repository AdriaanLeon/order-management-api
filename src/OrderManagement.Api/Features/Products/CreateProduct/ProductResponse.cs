using OrderManagement.Api.Domain.Products;

namespace OrderManagement.Api.Features.Products.CreateProduct;

public sealed record ProductResponse(
    Guid Id,
    string Name,
    string Sku,
    decimal Price,
    int Stock,
    bool IsActive)
{
    public static ProductResponse FromProduct(Product product)
    {
        return new ProductResponse(
            product.Id,
            product.Name,
            product.Sku,
            product.Price,
            product.Stock,
            product.IsActive);
    }
}