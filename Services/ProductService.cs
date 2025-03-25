using Bogus.DataSets;
using ECommerceCatalog.DTOs;
using ECommerceCatalog.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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

        // Get average Rating for a product
        public async Task<ProductDTO> GetAverageRatingForProductAsync(int id)
        {
            var productWithRatings = await _context.Products.Include(product => product.Reviews).FirstOrDefaultAsync(product => product.Id == id);

            if (productWithRatings == null) return null;

            var averageRating = productWithRatings.Reviews.Any() ? productWithRatings.Reviews.Average(review => review.Rating) : 0;

            var safeAverageRating = averageRating.GetValueOrDefault();

            var productDto = new ProductDTO
            {
                Id = productWithRatings.Id,
                Name = productWithRatings.Name,
                Price = productWithRatings.Price,
                AverageRating = safeAverageRating
            };

            return productDto;
        }

        // Filter product by category
        public async Task<IEnumerable<ProductDTO>> FilterProductsByCategoryNameAsync(string categoryName)
        {
            // Filter products based on category name
            var productsInCategory = await _context.Products.Include(product => product.Category).Where(product => product.Category != null && product.Category.Name.ToLower() == categoryName.ToLower()).ToListAsync();

            // Map filtered products to ProductDTO
            var productDto = productsInCategory.Select(product => new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                CategoryName = product.Category != null ? product.Category.Name : "No Category" // Safely access Category.Name
            }).ToList();

            return productDto;
        }
    }
}