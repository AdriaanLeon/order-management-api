using OrderManagement.Api.Domain.Common;
using OrderManagement.Api.Domain.Products;
using OrderManagement.Api.Infrastructure.Persistence;

namespace OrderManagement.Api.Features.Products.CreateProduct;

public sealed class CreateProductHandler(
    InMemoryProductStore productStore)
{
    public ProductResponse Handle(CreateProductRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (productStore.ExistsBySku(request.Sku))
        {
            throw new ConflictException(
                $"A product with SKU '{request.Sku.Trim().ToUpperInvariant()}' already exists.");
        }

        var product = Product.Create(
            request.Name,
            request.Sku,
            request.Price,
            request.InitialStock);

        productStore.Add(product);

        return ProductResponse.FromProduct(product);
    }
}