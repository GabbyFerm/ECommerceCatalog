using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerceCatalog.Models;
using ECommerceCatalog.Services;
using ECommerceCatalog.DTOs;
using AutoMapper;
using FluentValidation;

namespace ECommerceCatalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, ICategoryService categoryService, IMapper mapper)
        {
            _productService = productService;
            _categoryService = categoryService;
            _mapper = mapper;
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
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            var productById = await _productService.GetProductByIdAsync(id);

            if (productById == null) return NotFound();

            return Ok(productById);
        }

        // GET: api/Products/average rating
        [HttpGet("{id}/average-rating")]
        public async Task<ActionResult<ProductDTO>> GetAverageRatingForProduct(int id)
        {
            var productDto = await _productService.GetAverageRatingForProductAsync(id);

            if (productDto == null) return NotFound();

            return Ok(productDto);
        }

        // GET: api/Products/by category 
        [HttpGet("category/{categoryName}")]
        public async Task<ActionResult<ProductDTO>> FilterProductsByCategoryName(string categoryName)
        {
            var productDto = await _productService.FilterProductsByCategoryNameAsync(categoryName);

            if (productDto == null || !productDto.Any()) return NotFound();

            return Ok(productDto);
        }

        // Update: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDTO productToUpdate)
        {
            // Get the product from the database
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null) return NotFound();

            // Update the product properties
            product.Name = productToUpdate.Name;
            product.Price = productToUpdate.Price;

            // Handle Category update (by CategoryName or CategoryId)
            if (!string.IsNullOrEmpty(productToUpdate.CategoryName))
            {
                var category = await _categoryService.GetCategoryByNameAsync(productToUpdate.CategoryName);
                if (category != null)
                {
                    product.Category = category;  // Update the category if found
                }
                else
                {
                    return BadRequest("Category not found");  // Return a bad request if the category doesn't exist
                }
            }

            // Save the updated product
            var updatedProduct = await _productService.UpdateProductAsync(id, product);

            // If the product was updated successfully, map to DTO
            var productDto = _mapper.Map<ProductDTO>(updatedProduct);
            productDto.CategoryName = updatedProduct.Category?.Name; // Safely set CategoryName

            return Ok(productDto);
        }

        // Create: api/Products
        [HttpPost]
        public async Task<ActionResult<ProductDTO>> CreateProduct([FromBody] CreateProductDTO request, [FromServices] IValidator<CreateProductDTO> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var category = await _categoryService.GetCategoryByNameAsync(request.CategoryName);
            if (category == null)
            {
                category = new Category { Name = request.CategoryName };
                await _categoryService.CreateCategoryAsync(category);
            }

            var product = _mapper.Map<Product>(request);
            product.CategoryId = category.Id;

            var createdProduct = await _productService.CreateProductAsync(product);

            var productDto = _mapper.Map<ProductDTO>(createdProduct);
            productDto.CategoryName = category.Name;

            return Ok(productDto);
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
