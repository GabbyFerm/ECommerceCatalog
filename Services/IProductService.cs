using ECommerceCatalog.DTOs;
using ECommerceCatalog.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceCatalog.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product> CreateProductAsync(Product product);
        Task<Product> UpdateProductAsync(int id, Product product);
        Task<bool> DeleteProductAsync(int id);
        Task<ProductDTO> GetAverageRatingForProductAsync(int id);
        Task<IEnumerable<ProductDTO>> FilterProductsByCategoryNameAsync(string categoryName);
    }
}