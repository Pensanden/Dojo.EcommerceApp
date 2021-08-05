using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/Controller")]
    public class ProductsController : ControllerBase
    {
        private readonly StoreContext _context;

        public ProductsController(StoreContext context)
        {
             _context = context;   
        }

        [HttpGet("Products")]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();  
            return Ok(products);
        }

        [HttpGet("Product/{id}")]
        public async Task<ActionResult<Product>> GetProductAsync(int id)
        {
            var products = await _context.Products.FindAsync(id);  
            return Ok(products);
        }

    }
}