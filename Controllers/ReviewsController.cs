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
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly IProductService _productService;

        public ReviewsController(IReviewService reviewService, IProductService productService)
        {
            _reviewService = reviewService;
            _productService = productService;
        }

        // GET: api/Reviews
        [HttpGet("get-all-reviews")]
        public async Task<ActionResult> GetAllReviews()
        {
            var reviews = await _reviewService.GetAllReviewsAsync();

            return Ok(reviews);
        }

        // GET: api/Reviews/5
        [HttpGet("get-review-by-id")]
        public async Task<ActionResult> GetReviewById(int id)
        {
            var reviewById = await _reviewService.GetReviewByIdAsync(id);

            if (reviewById == null) return NotFound();

            return Ok(reviewById);
        }

        // PUT: api/Reviews/5
        [HttpPut("update-review-by-id")]
        public async Task<ActionResult> UpdateReview(int id, [FromBody] UpdateReviewDTO reviewDto, [FromServices] IValidator<UpdateReviewDTO> validator)
        {
            var validationResult = await validator.ValidateAsync(reviewDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var existingReview = await _reviewService.GetReviewByIdAsync(id);
            if (existingReview == null)
            {
                return NotFound("Review not found.");
            }

            // Update the existing review
            existingReview.Rating = reviewDto.Rating;
            existingReview.Comment = reviewDto.Comment;

            var updatedReview = await _reviewService.UpdateReviewAsync(id, existingReview); 

            return Ok(updatedReview);
        }

        // POST: api/Reviews
        [HttpPost("create-review")]
        public async Task<ActionResult> CreateReview([FromBody] CreateReviewDTO reviewToAddDto, [FromServices] IValidator<CreateReviewDTO> validator)
        {
            var validationResult = await validator.ValidateAsync(reviewToAddDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var product = await _productService.GetProductByIdAsync(reviewToAddDto.ProductId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            var review = new Review
            {
                ProductId = reviewToAddDto.ProductId,
                Rating = reviewToAddDto.Rating,
                Comment = reviewToAddDto.Comment,
                Product = product
            };

            var newReview = await _reviewService.CreateReviewAsync(review);

            return Ok(newReview);
        }

        // DELETE: api/Reviews/5
        [HttpDelete("delete-review")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var reviewToDelete = await _reviewService.DeleteReviewAsync(id);
            if (!reviewToDelete) return NotFound();

            return NoContent(); // Return 204 No content on succesful delete
        }
    }
}
