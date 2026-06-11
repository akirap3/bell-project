using Microsoft.EntityFrameworkCore;
using BellProject.Domain.Entities;

namespace BellProject.Infrastructure.Data
{
    /// <summary>
    /// Entity Framework Core Database Context for the application.
    /// Manages database connection and maps entity classes to database schemas.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Context constructor that accepts configuration options (e.g. database provider and connection string).
        /// </summary>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// DbSet representing the Products table.
        /// </summary>
        public DbSet<Product> Products => Set<Product>();

        /// <summary>
        /// Overrides model creation to apply Fluent API configurations and column constraints.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure properties and validations for Product entity mapping
            modelBuilder.Entity<Product>(entity =>
            {
                // Specify primary key
                entity.HasKey(e => e.Id);

                // Enforce length and non-null constraints
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                
                // Define financial decimal column precision (18 digits total, 2 decimal places)
                entity.Property(e => e.Price).HasPrecision(18, 2);
                entity.Property(e => e.Category).IsRequired().HasMaxLength(50);
            });
        }
    }
}
