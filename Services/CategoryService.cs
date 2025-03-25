using ECommerceCatalog.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceCatalog.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly EcommerceCatalogContext _context;

        public CategoryService(EcommerceCatalogContext context)
        {
            _context = context;
        }

        // Get all categories
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        // Get a category by id
        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        // Create a category
        public async Task<Category> CreateCategoryAsync(Category newCategory)
        {
            await _context.Categories.AddAsync(newCategory); 
            await _context.SaveChangesAsync(); 
            return newCategory; // Return the added category
        }

        // Update a category by id
        public async Task<Category?> UpdateCategoryAsync(int id, Category categoryToUpdate)
        {
            var existingCategory = await _context.Categories.FindAsync(id);
            if (existingCategory == null) return null;

            existingCategory.Name = categoryToUpdate.Name;

            await _context.SaveChangesAsync();
            return existingCategory;
        }

        // Delete a category by id
        public async Task<bool> DeleteCategoryAsync(int id) 
        {
            var categoryToDelete = await _context.Categories.FindAsync(id);
            if(categoryToDelete == null) return false;

            _context.Categories.Remove(categoryToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}