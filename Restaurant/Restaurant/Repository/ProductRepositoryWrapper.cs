using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Repository
{
    public class ProductRepositoryWrapper : IProductRepository
    {
        readonly IDistributedCache _distributedCache;
        readonly ProductRepository _productRepository;

        public ProductRepositoryWrapper(ProductRepository productRepository, IDistributedCache distributedCache)
        {
            _productRepository = productRepository;
            _distributedCache = distributedCache;
        }

        public async Task InvalidateCacheAsync(string key)
        {
            await _distributedCache.RemoveAsync(key);
        }

        public async Task<IEnumerable<string>> GetAllAsync()
        {
            var cacheResult = await _distributedCache.GetStringAsync("values");

            if (!string.IsNullOrWhiteSpace(cacheResult))
                return JsonConvert.DeserializeObject<IEnumerable<string>>(cacheResult);

            var dataFromDatabase = await _productRepository.GetAllAsync();

            await _distributedCache.SetStringAsync("values", JsonConvert.SerializeObject(dataFromDatabase), new DistributedCacheEntryOptions()
            {
                SlidingExpiration = new System.TimeSpan(0, 1, 0)
            });

            return dataFromDatabase;
        }
    }
}