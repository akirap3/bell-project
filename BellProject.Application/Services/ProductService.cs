using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BellProject.Application.DTOs;
using BellProject.Application.Exceptions;
using BellProject.Application.Interfaces;
using BellProject.Domain.Entities;
using BellProject.Domain.Repositories;

namespace BellProject.Application.Services
{
    /// <summary>
    /// Business logic service implementing Product operations.
    /// Acts as an orchestrator between repository layer data models and application DTOs.
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// Constructor executing dependency injection injection of the product repository.
        /// </summary>
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Retrieves a product by ID and maps it to a DTO.
        /// </summary>
        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return null;

            return MapToDto(product);
        }

        /// <summary>
        /// Retrieves all products and maps them to a DTO list.
        /// </summary>
        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.ListAllAsync();
            return products.Select(MapToDto);
        }

        /// <summary>
        /// Creates a new Product entity, saves it to database, and returns the DTO.
        /// </summary>
        public async Task<ProductDto> CreateProductAsync(CreateProductDto createDto)
        {
            var product = new Product
            {
                Name = createDto.Name,
                Description = createDto.Description ?? string.Empty,
                Price = createDto.Price,
                Stock = createDto.Stock,
                Category = createDto.Category,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdProduct = await _productRepository.AddAsync(product);
            return MapToDto(createdProduct);
        }

        /// <summary>
        /// Updates an existing Product entity. Throws NotFoundException if not found.
        /// </summary>
        public async Task UpdateProductAsync(UpdateProductDto updateDto)
        {
            var existing = await _productRepository.GetByIdAsync(updateDto.Id);
            if (existing == null)
            {
                throw new NotFoundException(nameof(Product), updateDto.Id);
            }

            existing.Name = updateDto.Name;
            existing.Description = updateDto.Description ?? string.Empty;
            existing.Price = updateDto.Price;
            existing.Stock = updateDto.Stock;
            existing.Category = updateDto.Category;
            existing.UpdatedAt = DateTime.UtcNow;

            await _productRepository.UpdateAsync(existing);
        }

        /// <summary>
        /// Deletes a Product entity. Throws NotFoundException if not found.
        /// </summary>
        public async Task DeleteProductAsync(int id)
        {
            var existing = await _productRepository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new NotFoundException(nameof(Product), id);
            }

            await _productRepository.DeleteAsync(existing);
        }

        /// <summary>
        /// Utility helper to map a Product entity to a ProductDto.
        /// </summary>
        private static ProductDto MapToDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                Category = product.Category,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };
        }
    }
}
