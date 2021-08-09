using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productsRepo,
            IGenericRepository<ProductType> productTypeRepo,
            IGenericRepository<ProductBrand> productBrandRepo,
            IMapper mapper)
        {
            _productsRepo = productsRepo;
            _productTypeRepo = productTypeRepo;
            _productBrandRepo = productBrandRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDTO>>> GetProducts([FromQuery] ProductSpecParams productPrams)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(productPrams);
            
            var countSpec = new ProductWithFiltersForCountSpecification(productPrams);
            
            var totalItems = await _productsRepo.CountAsync(countSpec);
            
            var products = await _productsRepo.ListAsync(spec);  
            
            var data = _mapper
                .Map<IReadOnlyList<Product>,IReadOnlyList<ProductToReturnDTO>>(products);
            
            return Ok(new Pagination<ProductToReturnDTO>(productPrams.PageIndex,productPrams.PageSize,totalItems,data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDTO>> GetProductAsync(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);

            var product = await _productsRepo.GetEntityWithSpec(spec); 
            
            if (product == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Product,ProductToReturnDTO>(product);
        }

        [HttpGet("ProductBrands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrandsAsync()
        {
            var brands = await _productBrandRepo.ListAllAsync();
            return Ok(brands);
        }

        [HttpGet("ProductTypes")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetTypesAsync()
        {
            var types = await _productTypeRepo.ListAllAsync();
            return Ok(types);                    
        }
    }
}