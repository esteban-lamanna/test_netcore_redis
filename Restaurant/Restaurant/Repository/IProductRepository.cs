using Restaurant.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<string>> GetAllAsync();
        Task InvalidateCacheAsync(string key);
        Task<Product> GetByNameAsync(string name);
        Task UpdateProductQuantityAsync(Product product);
    }
}