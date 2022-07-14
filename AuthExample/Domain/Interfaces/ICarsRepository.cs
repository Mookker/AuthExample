using AuthExample.Domain.Entities;

namespace AuthExample.Interfaces;

//TODO: it can be inherited from general repository
public interface ICarsRepository
{
    /// <summary>
    /// Gets all items
    /// </summary>
    /// <param name="offset">Number of items to skip</param>
    /// <param name="limit">Number of items to return</param>
    /// <returns>List of cars</returns>
    Task<List<Car>> GetAll(int offset = 0, int limit = 10);
}
