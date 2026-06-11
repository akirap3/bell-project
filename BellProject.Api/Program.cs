using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using BellProject.Application.Interfaces;
using BellProject.Application.Services;
using BellProject.Domain.Repositories;
using BellProject.Domain.Entities;
using BellProject.Infrastructure.Data;
using BellProject.Infrastructure.Repositories;

// Initialize the web application builder to setup logging, configurations, and Dependency Injection container
var builder = WebApplication.CreateBuilder(args);

// Register controller services for routing, validation and JSON serialization/formatting
builder.Services.AddControllers();

// Retrieve connection string and register the EF Core DbContext using SQL Server provider
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("BellProject.Infrastructure")));

// Register Repository and Service layers dependencies with Scoped lifetime (created per HTTP request)
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

// Register CORS service and define a policy allowing local Angular frontend origins
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularClient", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Compile configuration and build the WebApplication pipeline instance
var app = builder.Build();

// Create a temporary DI scope to execute database initialization and data seeding on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        // Automatically check database schema and seed default products if empty
        DbInitializer.Initialize(context);
    }
    catch (System.Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// HTTP request execution pipeline configuration (Middleware stack)

// Redirect all HTTP requests to secure HTTPS endpoints
app.UseHttpsRedirection();

// Apply the CORS policy named AllowAngularClient
app.UseCors("AllowAngularClient");

// Apply authorization mechanisms (roles/permissions check, if defined)
app.UseAuthorization();

// Scan controllers routes and map them to routing table
app.MapControllers();

// Start web server and listen to HTTP/HTTPS ports
app.Run();

