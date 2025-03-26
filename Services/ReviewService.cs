using ECommerceCatalog.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceCatalog.Services
{
    public class ReviewService : IReviewService
    {
        private readonly EcommerceCatalogContext _context;

        public ReviewService(EcommerceCatalogContext context) 
        { 
            _context = context;
        }

        // Get all reviews
        public async Task<IEnumerable<Review>> GetAllReviewsAsync() 
        { 
            return await _context.Reviews.ToListAsync();
        }

        public async Task<Review?> GetReviewByIdAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id); 
            return review; 
        }

        public async Task<Review> CreateReviewAsync(Review newReview) 
        { 
            await _context.Reviews.AddAsync(newReview);
            await _context.SaveChangesAsync();
            return newReview;
        }

        public async Task<Review> UpdateReviewAsync(int id, Review updatedReview)
        {
            var existingReview = await _context.Reviews.FindAsync(id);
            if (existingReview == null) return null;

            existingReview.Rating = updatedReview.Rating;
            existingReview.Comment = updatedReview.Comment;

            await _context.SaveChangesAsync();
            return existingReview;
        }

        public async Task<bool> DeleteReviewAsync(int id) 
        {
            var reviewToDelete = await _context.Reviews.FindAsync(id);
            if (reviewToDelete == null) return false;

            _context.Reviews.Remove(reviewToDelete); // Remove the review
            await _context.SaveChangesAsync(); // Save changes
            return true; // Return true if delteion is successful
        }
    }
}