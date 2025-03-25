using ECommerceCatalog.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceCatalog.Services
{
    public class ProductService : IProductService
    {
        private readonly EcommerceCatalogContext _context;

        public ProductService(EcommerceCatalogContext context)
        {
            _context = context;
        }

        // Get all products with their categories
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.Include(product => product.Category).ToListAsync();
        }

        // Get a product by id including its category
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products.Include(product => product.Category).FirstOrDefaultAsync(product => product.Id == id);
        }

        // Create a new product
        public async Task<Product> CreateProductAsync(Product newProduct)
        {
            await _context.Products.AddAsync(newProduct); 
            await _context.SaveChangesAsync(); 
            return newProduct; // Return the entity after it's added
        }

        // Update a product by id
        public async Task<Product?> UpdateProductAsync(int id, Product productToUpdate) 
        { 
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null) return null;

            existingProduct.Name = productToUpdate.Name;
            existingProduct.Price = productToUpdate.Price;
            existingProduct.CategoryId = productToUpdate.CategoryId;

            await _context.SaveChangesAsync();
            return existingProduct;
        }

        // Delete a product by id
        public async Task<bool> DeleteProductAsync(int id) 
        { 
            var productToDelete = await _context.Products.FindAsync(id);
            if (productToDelete == null) return false;

            _context.Products.Remove(productToDelete); // Remove product
            await _context.SaveChangesAsync(); // Save changes
            return true; // Return true if deleting is successful
        }
    }
}