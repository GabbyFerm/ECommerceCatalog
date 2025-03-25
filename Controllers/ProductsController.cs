using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerceCatalog.Models;
using ECommerceCatalog.Services;

namespace ECommerceCatalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult> GetProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetProductById(int id)
        {
            var productById = await _productService.GetProductByIdAsync(id);

            if (productById == null) return NotFound();

            return Ok(productById);
        }

        // Update: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product productToUpdate)
        {
            var updatedProduct = await _productService.UpdateProductAsync(id, productToUpdate);

            if (updatedProduct == null) return NotFound();

            return Ok(updatedProduct);
        }

        // Create: api/Products
        [HttpPost]
        public async Task<ActionResult> CreateProduct(Product productToAdd)
        {
            var newProduct = await _productService.CreateProductAsync(productToAdd);

            return Ok(newProduct);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var productToDelete = await _productService.DeleteProductAsync(id);
            if(!productToDelete) return NotFound(); // Return 404 if the product isnt found

            return NoContent(); // Return 204 No content on sucessful delete
        }
    }
}
