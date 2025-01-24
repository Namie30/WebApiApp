using Microsoft.EntityFrameworkCore;
using Domain;

namespace Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId);
        }

        // Seed Data Method
        public static void SeedData(AppDbContext context)
        {
            try
            {
                if (!context.Categories.Any())
                {
                    // Create Categories
                    var electronics = new Category { Name = "Electronics" };
                    var groceries = new Category { Name = "Groceries" };

                    // Add Categories to the Context
                    context.Categories.AddRange(electronics, groceries);

                    // Add Products to the Context
                    context.Products.AddRange(
                        new Product { Name = "Laptop", Price = 1200, Category = electronics },
                        new Product { Name = "Smartphone", Price = 800, Category = electronics },
                        new Product { Name = "Apple", Price = 1.2m, Category = groceries },
                        new Product { Name = "Milk", Price = 2.5m, Category = groceries }
                    );

                    // Save Changes
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding database: {ex.Message}");
                // Optionally: Log the error to a file or monitoring system
            }
        }
    }
}
