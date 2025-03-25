using Bogus;
using ECommerceCatalog.Models;

namespace ECommerceCatalog.Seeders
{
    public class ProductSeeder
    {
        private readonly EcommerceCatalogContext _context;

        public ProductSeeder(EcommerceCatalogContext context)
        {
            _context = context;
        }

        public void SeedProducts(int count)
        {
            if (!_context.Categories.Any()) return; // Ensure categories exist
            var existingProductCount = _context.Products.Count();
            if (existingProductCount >= count) return; // Skip if enough products exist

            var categories = _context.Categories.ToList(); // Fetch existing categories

            var faker = new Faker<Product>()
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Price, f => f.Random.Decimal(1, 1000))
                .RuleFor(p => p.CategoryId, f => f.PickRandom(categories).Id); // Assign a valid CategoryId

            var products = faker.Generate(count - existingProductCount);

            _context.Products.AddRange(products);
            _context.SaveChanges();
        }
    }
}