using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Restaurant.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        public TestController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok($"Hello from IP: [{HttpContext.Connection.RemoteIpAddress}]" +
                      $"host: [{HttpContext.Request.Host.Host}]");
        }

        [Route("Config")]
        [HttpGet]
        public async Task<IActionResult> GetConfig()
        {
            return Ok($"Connection string: [{Configuration.GetConnectionString("sqlServer")}]\n" +
                      $"test-child1: [{Configuration.GetSection("Test:Child1").Value}]\n" +
                      "");
        }
    }
}