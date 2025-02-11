using Microsoft.AspNetCore.Mvc;
using MyAdminifyApp.Api.DTOs;
using MyAdminifyApp.Application.Interfaces;
using MyAdminifyApp.Domain.Entities;

namespace MyAdminifyApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {

        private readonly IProdutService _productService;

        public ProductController(IProdutService productService)
        {
            _productService = productService;
        }



        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var products = _productService.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound($"Product with id {id} not found.");
            }
            return Ok(product);
        }
        [HttpPost]
        public IActionResult AddProduct(ProductDTO productDto)
        {
            if (string.IsNullOrWhiteSpace(productDto.Name))
            {
                return BadRequest("Product name cannot be empty.");
            }
            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price
            };
            _productService.AddProduct(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, ProductDTO updatedProductDTO)
        {
            var existingProduct = _productService.GetProductById(id);
            if (existingProduct == null)
            {
                return NotFound($"Product with id {id} not found.");
            }

            existingProduct.Name = updatedProductDTO.Name;
            existingProduct.Price = updatedProductDTO.Price;


            _productService.UpdateProduct(existingProduct);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound($"Product with id {id} not found.");
            }

            _productService.DeleteProduct(id);
            return NoContent();
        }
    }
}
