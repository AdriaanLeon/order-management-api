namespace OrderManagement.Api.Features.Products.CreateProduct;

public sealed record CreateProductRequest(
    string Name,
    string Sku,
    decimal Price,
    int InitialStock);