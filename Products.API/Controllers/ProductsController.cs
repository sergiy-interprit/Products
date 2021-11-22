using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Products.API.Models;
using Products.API.Validations;
using Products.Core.Models;
using Products.Core.Services;

namespace Products.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        
        public ProductsController(IProductService productService, IMapper mapper)
        {
            this._mapper = mapper;
            this._productService = productService;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
        {
            var products = await _productService.GetAllWithAccount();
            var productDtos = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);

            return Ok(productDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var product = await _productService.GetProductById(id);
            var productDto = _mapper.Map<Product, ProductDto>(product);

            return Ok(productDto);
        }

        [HttpPost("")]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] SaveProductDto saveProductDto)
        {
            var validator = new SaveProductDtoValidator();
            var validationResult = await validator.ValidateAsync(saveProductDto);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors); // this needs refining, but for demo it is ok

            var productToCreate = _mapper.Map<SaveProductDto, Product>(saveProductDto);
            var newProduct = await _productService.CreateProduct(productToCreate);

            var product = await _productService.GetProductById(newProduct.Id);
            var productDto = _mapper.Map<Product, ProductDto>(product);

            return Ok(productDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(int id, [FromBody] SaveProductDto saveProductDto)
        {
            var validator = new SaveProductDtoValidator();
            var validationResult = await validator.ValidateAsync(saveProductDto);
            
            var requestIsInvalid = id == 0 || !validationResult.IsValid;

            if (requestIsInvalid)
                return BadRequest(validationResult.Errors); // this needs refining, but for demo it is ok
            
            var productToBeUpdate = await _productService.GetProductById(id);

            if (productToBeUpdate == null)
                return NotFound();

            var product = _mapper.Map<SaveProductDto, Product>(saveProductDto);
            await _productService.UpdateProduct(productToBeUpdate, product);

            var updatedProduct = await _productService.GetProductById(id);
            var updatedProductDto = _mapper.Map<Product, ProductDto>(updatedProduct);
    
            return Ok(updatedProductDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (id == 0)
                return BadRequest();
            
            var product = await _productService.GetProductById(id);

            if (product == null)
                return NotFound();

            await _productService.DeleteProduct(product);

            return NoContent();
        }
    }
}