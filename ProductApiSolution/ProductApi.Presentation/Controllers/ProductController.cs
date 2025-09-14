using eCommerce.Shared.Library.Responses;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTOs;
using ProductApi.Application.DTOs.Conversions;
using ProductApi.Application.Interfaces;

namespace ProductApi.Presentation.Controllers;

public class ProductController ( IProduct productInterface) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts()
    {
        var products = await productInterface.GetAllAsync();
        if (!products.Any()) return NotFound("No product found");
        var (_, list) = ProductConversions.FromEntity(null, products);
        return list.Any() ? Ok(list) : NotFound("No product found");
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDTO>> GetProductById(int productId)
    {
        var product = await productInterface.FindByIdAsync(productId);
        var (_product, _) = ProductConversions.FromEntity(product, null!);
        return _product is not null ? Ok(_product) : NotFound("Product requested not found");
    }

    [HttpPost]
    public async Task<ActionResult<Response>> CreateNewProduct(ProductDTO productDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var getEntity = ProductConversions.ToEntity(productDto);
        var response = await productInterface.CreateAsync(getEntity);
        return response.Flag is true ? Ok(response) : BadRequest(response);
    }

    [HttpPut]
    public async Task<ActionResult<Response>> UpdateProduct(ProductDTO productDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var getEntity = ProductConversions.ToEntity(productDto);
        var response = await productInterface.UpdateAsync(getEntity);
        return response.Flag is true ? Ok(response) : BadRequest(response);
    }

    [HttpDelete]
    public async Task<ActionResult<Response>> DeleteProduct(ProductDTO productDto)
    {
        var getEntity = ProductConversions.ToEntity(productDto);
        var response = await productInterface.DeleteAsync(getEntity);
        return response.Flag is true ? Ok(response) : BadRequest(response);
    }
    
}