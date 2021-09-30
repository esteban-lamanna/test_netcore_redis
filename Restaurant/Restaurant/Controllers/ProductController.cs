using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Logic;
using Restaurant.Models;
using Restaurant.Repository;
using System;
using System.Threading.Tasks;

namespace Restaurant.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        readonly IProductLogic _productLogic;
        readonly IServiceProvider _serviceProvider;

        public ProductController(IProductRepository foodRepository, IProductLogic productLogic, IServiceProvider serviceProvider)
        {
            _productRepository = foodRepository;
            _productLogic = productLogic;
            _serviceProvider = serviceProvider;
        }

        [Route("invalidate")]
        [HttpDelete]
        public async Task<IActionResult> Invalidate([FromQuery] string key)
        {
            await _productRepository.InvalidateCacheAsync(key);

            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(new
            {
                data = await _productRepository.GetAllAsync()
            });
        }


        [HttpPost]
        public IActionResult Buy(BuyRequest buyRequest)
        {
            Parallel.For(0, 15, async (number, parallelState) =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var logic = scope.ServiceProvider.GetService<IProductLogic>();

                    try
                    {
                        await logic.Buy(buyRequest);

                        Console.WriteLine($"Iteration {number} completed");
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"Iteration {number} broken");
                        parallelState.Break();
                    }
                }
            });

            return Ok();
        }
    }
}
