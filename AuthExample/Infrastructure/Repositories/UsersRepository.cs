using AuthExample.Domain.Entities;
using AuthExample.Domain.Interfaces;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace AuthExample.Infrastructure.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IDbConnection _connection;
        public UsersRepository(IConfiguration configuration)
        {
            _connection = new SqlConnection(configuration.GetConnectionString("SqlConnectionString"));
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var user = await _connection.QuerySingleOrDefaultAsync<User>
                ($"SELECT * FROM Users WHERE Id = @Id", new { Id = id });

            return user;
        }

        public async Task<User> GetByLoginAsync(string login)
        {
            var user = await _connection.QuerySingleOrDefaultAsync<User>
                ($"SELECT * FROM Users WHERE Login = @Login", new { Login = login });

            return user;
        }

        public async Task<bool> UpdateBlockedAsync(int id, bool isBlocked)
        {
            var result = await _connection.ExecuteAsync(
                "UPDATE Users SET [IsBlocked]=@IsBlocked, [ModifiedDate]=@ModifiedDate WHERE Id=@Id",
                new
                {
                    IsBlocked = isBlocked,
                    ModifiedDate = DateTime.UtcNow,
                    Id = id
                });

            return result > 0;
        }

        public async Task<bool> UpdatePasswordAsync(int id, string passwordHash)
        {
            var result = await _connection.ExecuteAsync(
                "UPDATE Users SET [PasswordHash]=@PasswordHash, [ModifiedDate]=@ModifiedDate WHERE Id=@Id", 
                new 
                { 
                    PasswordHash = passwordHash, 
                    ModifiedDate = DateTime.UtcNow, 
                    Id = id 
                });

            return result > 0;
        }
    }
}
