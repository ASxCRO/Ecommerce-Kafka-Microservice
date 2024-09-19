using Ecommerce.Model;
using Ecommerce.ProductService.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(ProductDbContext productDbContext) : ControllerBase
    {
        [HttpGet]
        public async Task<List<ProductModel>> GetProducts()
        {
            return await productDbContext.Products.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ProductModel> GetProduct(int id)
        {
            return await productDbContext.Products.FindAsync(id);
        }
    }
}
