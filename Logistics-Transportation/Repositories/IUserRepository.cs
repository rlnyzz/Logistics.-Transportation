using Logistics_Transportation.Models;

namespace Logistics_Transportation.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(string id);

    }
}
