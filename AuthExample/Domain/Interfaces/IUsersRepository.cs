using AuthExample.Domain.Entities;

namespace AuthExample.Domain.Interfaces
{
    public interface IUsersRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<User> GetByLoginAsync(string login);
        Task<bool> UpdatePasswordAsync(int id, string passwordHash);
        Task<bool> UpdateBlockedAsync(int id, bool isBlocked);
    }
}
