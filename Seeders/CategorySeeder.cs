using Bogus;
using ECommerceCatalog.Models;

namespace ECommerceCatalog.Seeders
{
    public class CategorySeeder
    {
        private readonly EcommerceCatalogContext _context;

        public CategorySeeder(EcommerceCatalogContext context)
        {
            _context = context;
        }

        public void SeedCategories(int count)
        {
            if (_context.Categories.Any()) return; // Skip if categories exist

            var faker = new Faker<Category>()
                .RuleFor(c => c.Name, f => f.Commerce.Categories(1)[0]);

            var categories = faker.Generate(count);

            _context.Categories.AddRange(categories);
            _context.SaveChanges();
        }
    }
}
