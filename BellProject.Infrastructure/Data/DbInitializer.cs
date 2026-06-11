using System;
using System.Linq;
using BellProject.Domain.Entities;

namespace BellProject.Infrastructure.Data
{
    /// <summary>
    /// Static class responsible for seeding initial values to the database.
    /// Used for local development and demonstration purposes.
    /// </summary>
    public static class DbInitializer
    {
        /// <summary>
        /// Instantiates database if needed and seeds default Product data if empty.
        /// </summary>
        /// <param name="context">Active DbContext instance.</param>
        public static void Initialize(ApplicationDbContext context)
        {
            // Create database and schemas if they do not exist
            context.Database.EnsureCreated();

            // Return early if database already contains data to prevent duplicate seeds
            if (context.Products.Any())
            {
                return;
            }

            // Create array of default demo products
            var products = new Product[]
            {
                new() {
                    Name = "Aether Pro Keyboard",
                    Description = "Ultra-premium mechanical keyboard with custom linear switches and glass casing design.",
                    Price = 189.99m,
                    Stock = 45,
                    Category = "Peripherals",
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    UpdatedAt = DateTime.UtcNow.AddDays(-10)
                },
                new() {
                    Name = "Lumina Screen Lightbar",
                    Description = "Monitor mounted smart light bar with ambient backlighting and auto-dimming controls.",
                    Price = 79.50m,
                    Stock = 120,
                    Category = "Lighting",
                    CreatedAt = DateTime.UtcNow.AddDays(-7),
                    UpdatedAt = DateTime.UtcNow.AddDays(-7)
                },
                new() {
                    Name = "Vortex Ergonomic Chair",
                    Description = "State-of-the-art office chair with full lumbar adjustment and breathable premium mesh.",
                    Price = 499.00m,
                    Stock = 12,
                    Category = "Furniture",
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    UpdatedAt = DateTime.UtcNow.AddDays(-5)
                },
                new() {
                    Name = "Nexus Audio Interface",
                    Description = "Dual-channel USB interface with crystal-clear preamps and direct zero-latency monitoring.",
                    Price = 149.00m,
                    Stock = 30,
                    Category = "Audio",
                    CreatedAt = DateTime.UtcNow.AddDays(-3),
                    UpdatedAt = DateTime.UtcNow.AddDays(-3)
                },
                new() {
                    Name = "Nebula Desk Pad",
                    Description = "Sleek liquid-resistant micro-weave desk pad featuring stunning glassmorphic cosmic patterns.",
                    Price = 29.99m,
                    Stock = 200,
                    Category = "Accessories",
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                    UpdatedAt = DateTime.UtcNow.AddDays(-1)
                }
            };

            // Stage and commit data to database
            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}
