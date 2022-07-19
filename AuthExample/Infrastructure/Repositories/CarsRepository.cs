using AuthExample.Domain.Entities;
using AuthExample.Domain.Interfaces;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace AuthExample.Infrastructure.Repositories
{
    public class CarsRepository : ICarsRepository
    {
        private readonly IDbConnection _connection;
        public CarsRepository(IConfiguration configuration)
        {
            _connection = new SqlConnection(configuration.GetConnectionString("SqlConnectionString"));
        }

        /// <inheritdoc/>
        public async Task<List<Car>> GetAll(int offset = 0, int limit = 10)
        {
            var cars = await _connection.QueryAsync<Car>($"SELECT * FROM Cars OFFSET ORDER BY Id OFFSET {offset} ROWS FETCH NEXT {limit} ROWS ONLY");

            return cars.ToList();
        }
    }
}
