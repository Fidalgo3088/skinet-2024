using System;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IGenericRepository<Product> repo) : ControllerBase
{
    // private readonly StoreContext context = context;

    // public ProductsController(StoreContext context) 
    // {
    //     this.context = context;
    // }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? brand, string? type, string? sort)
    {
        // return await context.Products.ToListAsync();

        var spec = new ProductSpecification(brand, type, sort);
        var products = await repo.ListAsync(spec);
        return Ok(products);

    }

    [HttpGet("{id:int}")] //api/products/1
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        // var product = await context.Products.FindAsync(id);
        var product = await repo.GetByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }
        return product;
    }

    [HttpPost] 
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        // context.Products.Add(product);
        repo.Add(product);
        // await context.SaveChangesAsync();
        if (await repo.SaveAllAsync())        
        {
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }
        return BadRequest("Problem creating product");
    }

    [HttpPut("{id:int}")] //api/products/1  ~
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (product.Id != id! || !ProductsExists(id))
        {
            return BadRequest("Cannot update this product");
        }
            // context.Entry(product).State = EntityState.Modified;
            repo.Update(product);
            // await context.SaveChangesAsync();
            if (await repo.SaveAllAsync())        
            {
                return NoContent();
            }
            return BadRequest("Problem updating the product");
    }

    [HttpDelete("{id:int}")] //api/products/1
    public async Task<ActionResult> DeleteProduct(int id)
    {
        // var product = await context.Products.FindAsync(id);
        var product = await repo.GetByIdAsync(id);  // use repository method;
        if (product == null) { 
            return NotFound();
        }
        // context.Products.Remove(product);
        repo.Remove(product);
        if (await repo.SaveAllAsync()) 
        {
            return NoContent();
        }        

        // await context.SaveChangesAsync();
        // return NoContent();
        return BadRequest("Problem deleting the product");
    }

    [HttpGet("brands")] 
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        // return Ok(await repo.GetBrandsAsync());
        // Implement method
        var spec = new BrandListSpecification(); 
         return Ok(await repo.ListAsync(spec));

    }

    [HttpGet("types")] 
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        // return Ok(await repo.GetTypesAsync());
        // Implement method
        var spec = new TypeListSpecification(); 
         return Ok(await repo.ListAsync(spec));

    }
    private bool ProductsExists(int id)
    {
        // return context.Products.Any(x => x.Id == id);
        return repo.Exists(id);  // use repository method;
    }
}

