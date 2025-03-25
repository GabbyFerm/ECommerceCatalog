using ECommerceCatalog.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceCatalog.Services
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetAllReviewsAsync();
        Task<Review> GetReviewByIdAsync(int id);
        Task<Review> CreateReviewAsync(Review review);
        Task<Review> UpdateReviewAsync(int id, Review review);
        Task<bool> DeleteReviewAsync(int id);
    }
}