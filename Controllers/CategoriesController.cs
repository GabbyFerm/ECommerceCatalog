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
using FluentValidation;

namespace ECommerceCatalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService category)
        {
            _categoryService = category;
        }

        // GET: api/Categories
        [HttpGet("get-all-categories")]
        public async Task<ActionResult> GetCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();

            return Ok(categories);

        }

        // GET: api/Categories/5
        [HttpGet("get-category-by-id")]
        public async Task<ActionResult> GetCategory(int id)
        {
            var categoryById = await _categoryService.GetCategoryByIdAsync(id);

            if (categoryById == null) return NotFound();

            return Ok(categoryById);
        }

        // GET api/categories/name/{name}
        [HttpGet("get-category-by-name")]
        public async Task<ActionResult<Category>> GetCategoryByName(string name)
        {
            var category = await _categoryService.GetCategoryByNameAsync(name);

            if (category == null) return NotFound();

            return Ok(category);
        }

        // Update: api/Categories/5
        [HttpPut("update-category")]
        public async Task<ActionResult<CategoryDTO>> UpdateCategory(int id,[FromBody] UpdateCategoryDTO categoryDto, [FromServices] IValidator<UpdateCategoryDTO> validator)
        {
            var validationResult = await validator.ValidateAsync(categoryDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var existingCategory = await _categoryService.GetCategoryByIdAsync(id);
            if (existingCategory == null)
            {
                return NotFound("Category not found.");
            }

            existingCategory.Name = categoryDto.Name;
            var updatedCategory = await _categoryService.UpdateCategoryAsync(id, existingCategory);

            var categoryResponse = new CategoryDTO
            {
                Id = updatedCategory.Id,
                Name = updatedCategory.Name
            };

            return Ok(categoryResponse);
        }

        // Create: api/Categories
        [HttpPost("create-category")]
        public async Task<ActionResult> CreateCategory([FromBody] Category categoryToAdd)
        {
            var newCategory = await _categoryService.CreateCategoryAsync(categoryToAdd);

            return Ok(newCategory);
        }

        // DELETE: api/Categories/5
        [HttpDelete("delete-category-by-id")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var categoryToDelete = await _categoryService.DeleteCategoryAsync(id);

            if (!categoryToDelete) return NotFound(); // Return 404 if the product isnt found

            return NoContent(); // Return 204 No content on sucessful delete
        }
    }
}