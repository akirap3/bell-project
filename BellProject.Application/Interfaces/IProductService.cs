using System.Collections.Generic;
using System.Threading.Tasks;
using BellProject.Application.DTOs;

namespace BellProject.Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> CreateProductAsync(CreateProductDto createDto);
        Task UpdateProductAsync(UpdateProductDto updateDto);
        Task DeleteProductAsync(int id);
    }
}
