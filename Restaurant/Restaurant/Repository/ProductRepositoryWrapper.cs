using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Repository
{
    public class ProductRepositoryWrapper : IProductRepository
    {
        readonly IDistributedCache _distributedCache;
        readonly ProductRepository _productRepository;
        readonly ILogger<ProductRepositoryWrapper> _logger;
        readonly IConfiguration _configuration;

        public ProductRepositoryWrapper(ProductRepository productRepository, IDistributedCache distributedCache, ILogger<ProductRepositoryWrapper> logger, IConfiguration configuration)
        {
            _productRepository = productRepository;
            _distributedCache = distributedCache;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task InvalidateCacheAsync(string key)
        {
            await _distributedCache.RemoveAsync(key);
        }

        public async Task<IEnumerable<string>> GetAllAsync()
        {
            string cacheResult = null;

            try
            {
                cacheResult = await _distributedCache.GetStringAsync("values");
            }
            catch (Exception e)
            {
                var connection = _configuration.GetValue<string>("Redis:ConnectionString");
                _logger.LogError(e, $"Hubo un problema al conectarse a redis. [{connection}]");
            }

            if (!string.IsNullOrWhiteSpace(cacheResult))
                return JsonConvert.DeserializeObject<IEnumerable<string>>(cacheResult);

            var dataFromDatabase = await _productRepository.GetAllAsync();

            await _distributedCache.SetStringAsync("values", JsonConvert.SerializeObject(dataFromDatabase), new DistributedCacheEntryOptions()
            {
                SlidingExpiration = new System.TimeSpan(0, 1, 0)
            });

            return dataFromDatabase;
        }

        public Task<Product> GetByNameAsync(string name)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateProductQuantityAsync(Product product)
        {
            throw new System.NotImplementedException();
        }
    }
}