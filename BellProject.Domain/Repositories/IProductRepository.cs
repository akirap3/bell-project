using System.Collections.Generic;
using System.Threading.Tasks;
using BellProject.Domain.Entities;

namespace BellProject.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(int id);
        Task<IReadOnlyList<Product>> ListAllAsync();
        Task<Product> AddAsync(Product entity);
        Task UpdateAsync(Product entity);
        Task DeleteAsync(Product entity);
    }
}
