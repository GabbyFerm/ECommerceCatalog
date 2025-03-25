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
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        // GET: api/Reviews
        [HttpGet]
        public async Task<ActionResult> GetAllReviews()
        {
            var reviews = await _reviewService.GetAllReviewsAsync();

            return Ok(reviews);
        }

        // GET: api/Reviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetReviewById(int id)
        {
            var reviewById = await _reviewService.GetReviewByIdAsync(id);

            if (reviewById == null) return NotFound();

            return Ok(reviewById);
        }

        // PUT: api/Reviews/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReviewById(int id, [FromBody] Review reviewToUpdate)
        {
            var updatedReview = await _reviewService.UpdateReviewAsync(id, reviewToUpdate);

            if (updatedReview == null) return NotFound(); // If update fails, return 404

            return Ok(updatedReview); 
        }

        // POST: api/Reviews
        [HttpPost]
        public async Task<ActionResult> CreateReview([FromBody] Review reviewToAdd)
        {
            var newReview = await _reviewService.CreateReviewAsync(reviewToAdd);

            return Ok(newReview);
        }

        // DELETE: api/Reviews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var reviewToDelete = await _reviewService.DeleteReviewAsync(id);
            if (!reviewToDelete) return NotFound();

            return NoContent(); // Return 204 No content on succesful delete
        }
    }
}
