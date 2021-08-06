using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/Controller")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repo;

        public ProductsController(IProductRepository repo)
        {
             _repo = repo;   
        }

        [HttpGet("Products")]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _repo.GetProductsAsync();  
            return Ok(products);
        }

        [HttpGet("Product/{id}")]
        public async Task<ActionResult<Product>> GetProductAsync(int id)
        {
            var products = await _repo.GetProductsByIdAsync(id); 
            return Ok(products);
        }

    }
}