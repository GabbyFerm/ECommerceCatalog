using Bogus;
using ECommerceCatalog.Models;

namespace ECommerceCatalog.Seeders
{
    public class ReviewSeeder
    {
        private readonly EcommerceCatalogContext _context;

        public ReviewSeeder(EcommerceCatalogContext context)
        {
            _context = context;
        }

        public void SeedReviews(int count)
        {
            if (!_context.Products.Any()) return; // Ensure products exist
            var existingReviewCount = _context.Reviews.Count();
            if (existingReviewCount >= count) return;

            var products = _context.Products.ToList(); // Fetch existing products

            var faker = new Faker<Review>()
                .RuleFor(r => r.ProductId, f => f.PickRandom(products).Id) // Assign a valid ProductId
                .RuleFor(r => r.Rating, f => f.Random.Int(1, 5))
                .RuleFor(r => r.Comment, f => f.Lorem.Sentence());

            var reviews = faker.Generate(count - existingReviewCount);

            _context.Reviews.AddRange(reviews);
            _context.SaveChanges();
        }
    }
}
