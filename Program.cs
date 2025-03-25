using ECommerceCatalog.Models;
using ECommerceCatalog.Seeders;
using Microsoft.EntityFrameworkCore;

namespace ECommerceCatalog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Register the DbContext with the DI container
            builder.Services.AddDbContext<EcommerceCatalogContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register Seeders in DI
            builder.Services.AddScoped<CategorySeeder>();
            builder.Services.AddScoped<ProductSeeder>();
            builder.Services.AddScoped<ReviewSeeder>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            // Seed the database with test data
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<EcommerceCatalogContext>();
                try
                {
                    // Use DI to get seeders instead of manually creating instances
                    var categorySeeder = scope.ServiceProvider.GetRequiredService<CategorySeeder>();
                    categorySeeder.SeedCategories(5); // Generate 5 categories

                    var productSeeder = scope.ServiceProvider.GetRequiredService<ProductSeeder>();
                    productSeeder.SeedProducts(20); // Generate 20 products

                    var reviewSeeder = scope.ServiceProvider.GetRequiredService<ReviewSeeder>();
                    reviewSeeder.SeedReviews(50); // Generate 50 reviews
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
                }

            }

            app.Run();
        }
    }
}
