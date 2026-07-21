using Microsoft.AspNetCore.Mvc;
using OrderManagement.Api.Domain.Products;
using OrderManagement.Api.Features.Products.CreateProduct;
using OrderManagement.Api.Infrastructure.Persistence;

namespace OrderManagement.Api.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController(
    CreateProductHandler createProductHandler,
    InMemoryProductStore productStore)
    : ControllerBase
{
    [HttpPost]
    public ActionResult<ProductResponse> Create(
    [FromBody] CreateProductRequest request)
    {
        var response = createProductHandler.Handle(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = response.Id },
            response);
    }

    [HttpGet("{id:guid}")]
    public ActionResult<ProductResponse> GetById(Guid id)
    {
        var product = productStore.GetById(id);

        if (product is null)
        {
            return NotFound();
        }

        return Ok(ProductResponse.FromProduct(product));
    }
}