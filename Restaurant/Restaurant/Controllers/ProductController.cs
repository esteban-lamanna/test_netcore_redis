using Microsoft.AspNetCore.Mvc;
using Restaurant.Repository;
using System.Threading.Tasks;

namespace Restaurant.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository foodRepository)
        {
            _productRepository = foodRepository;
        }

        [Route("invalidate/{key}")]
        [HttpGet]
        public async Task<IActionResult> Invalidate(string key)
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
    }
}
