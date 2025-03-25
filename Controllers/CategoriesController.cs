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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService category)
        {
            _categoryService = category;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult> GetCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();

            return Ok(categories);

        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetCategory(int id)
        {
            var categoryById = await _categoryService.GetCategoryByIdAsync(id);

            if (categoryById == null) return NotFound();

            return Ok(categoryById);
        }

        // GET api/categories/name/{name}
        [HttpGet("name/{name}")]
        public async Task<ActionResult<Category>> GetCategoryByName(string name)
        {
            var category = await _categoryService.GetCategoryByNameAsync(name);

            if (category == null) return NotFound();

            return Ok(category);
        }

        // Update: api/Categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category categoryToUpdate)
        {
            var updatedCategory = await _categoryService.UpdateCategoryAsync(id, categoryToUpdate);

            if (updatedCategory == null) return NotFound();

            return Ok(updatedCategory);
        }

        // Create: api/Categories
        [HttpPost]
        public async Task<ActionResult> CreateCategory([FromBody] Category categoryToAdd)
        {
            var newCategory = await _categoryService.CreateCategoryAsync(categoryToAdd);

            return Ok(newCategory);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var categoryToDelete = await _categoryService.DeleteCategoryAsync(id);

            if (!categoryToDelete) return NotFound(); // Return 404 if the product isnt found

            return NoContent(); // Return 204 No content on sucessful delete
        }
    }
}