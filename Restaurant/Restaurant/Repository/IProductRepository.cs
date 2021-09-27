using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<string>> GetAllAsync();
        Task InvalidateCacheAsync(string key);
    }
}