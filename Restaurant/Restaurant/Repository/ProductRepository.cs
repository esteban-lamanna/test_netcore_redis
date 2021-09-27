using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Repository
{
    public class ProductRepository : IProductRepository
    {
        public string _connectionString;

        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("sqlServer");
        }

        public async Task<IEnumerable<string>> GetAllAsync()
        {
            var sql = "SELECT name FROM Product;";

            await Task.Delay(3000);

            using (var connection = new Microsoft.Data.SqlClient.SqlConnection(_connectionString))
            {
                connection.Open();

                return connection.Query<string>(sql);
            }
        }

        public Task InvalidateCacheAsync(string key)
        {
            return Task.CompletedTask;
        }
    }
}