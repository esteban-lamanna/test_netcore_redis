using Microsoft.Extensions.Logging;
using Restaurant.Models;
using Restaurant.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurant.Logic
{
    public class ProductLogic : IProductLogic
    {
        readonly IProductRepository _productRepository;
        readonly ILogger<ProductLogic> _logger;
        static readonly object BuyingLockingObject = new object();

        public ProductLogic(IProductRepository productRepository, ILogger<ProductLogic> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }
        ~ProductLogic()
        {
            _logger.LogError($"asdasdasdassad");
        }

        public async Task<Product> Buy(BuyRequest buyRequest)
        {
            lock (BuyingLockingObject)
            {
                var productFromDB = _productRepository.GetByNameAsync(buyRequest.ProductName).GetAwaiter().GetResult();

                productFromDB.Quantity -= buyRequest.Quantity;

                if (productFromDB.Quantity < 0)
                {
                    _logger.LogWarning($"Thread- {Thread.CurrentThread.ManagedThreadId} - Cantidad menor a cero. Product " +
                        $"{productFromDB.Name} - Quantity left {productFromDB.Quantity} - Quantity sold - 0");

                    throw new System.Exception($"Cantidad menor a cero.");
                }

                //simulate long operation
                Task.Delay(300);

                _productRepository.UpdateProductQuantityAsync(productFromDB).GetAwaiter().GetResult();

                _logger.LogWarning($"Thread- {Thread.CurrentThread.ManagedThreadId} - Sold {productFromDB.Name} - " +
                    $"Quantity left {productFromDB.Quantity} - Quantity sold - {buyRequest.Quantity}");

                return productFromDB;
            }
        }
    }
}