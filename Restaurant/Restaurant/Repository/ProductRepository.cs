using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Restaurant.Models;
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

        public async Task<Product> GetByNameAsync(string name)
        {
            var sql = $"SELECT * FROM Product where name = '{name}';";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return await connection.QueryFirstAsync<Product>(sql);
            }
        }

        public async Task UpdateProductQuantityAsync(Product product)
        {
            var sql = $"UPDATE Product SET Quantity = {product.Quantity} where name = '{product.Name}';";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                await connection.ExecuteAsync(sql);
            }
        }

        public async Task<IEnumerable<string>> GetAllAsync()
        {
            var sql = "SELECT name FROM Product;";

            await Task.Delay(3000);

            using (var connection = new SqlConnection(_connectionString))
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