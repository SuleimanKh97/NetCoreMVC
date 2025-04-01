using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestMasterPeace.DTOs.ProductsDTOs;
using TestMasterPeace.Models;

namespace TestMasterPeace.Controllers;

[Route("[controller]")]
[ApiController]
public class ProductController(MasterPeiceContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        return Ok(await context.Products.ToListAsync());
    }
    [HttpPost]
    public async Task<IActionResult> PostProduct(CreateProductRequest newProduct)
    {
        //await context.Products.AddAsync();
        //await context.SaveChangesAsync();
        return Ok();
    }
}
