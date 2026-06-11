using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BellProject.Application.DTOs;
using BellProject.Application.Exceptions;
using BellProject.Application.Interfaces;

namespace BellProject.Api.Controllers
{
    /// <summary>
    /// API Controller defining HTTP endpoints for CRUD operations on Products.
    /// Exposes endpoints under "/api/products".
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        /// <summary>
        /// Constructor executing dependency injection injection of the product service.
        /// </summary>
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Endpoint: GET /api/products
        /// Retrieves all products.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        /// <summary>
        /// Endpoint: GET /api/products/{id}
        /// Retrieves a single product by its unique integer ID.
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound(new { message = $"Product with ID {id} was not found." });
            }
            return Ok(product);
        }

        /// <summary>
        /// Endpoint: POST /api/products
        /// Creates a new product. Uses request body validation.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductDto createDto)
        {
            // Fallback check if ApiController model validation is not automatically executed
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdProduct = await _productService.CreateProductAsync(createDto);
            // Returns HTTP 201 with Location header pointing to GetById endpoint
            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
        }

        /// <summary>
        /// Endpoint: PUT /api/products/{id}
        /// Updates an existing product details. Returns HTTP 204 on success.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto updateDto)
        {
            if (id != updateDto.Id)
            {
                return BadRequest(new { message = "ID in path does not match ID in body." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _productService.UpdateProductAsync(updateDto);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Endpoint: DELETE /api/products/{id}
        /// Deletes a product. Returns HTTP 204 on success.
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
