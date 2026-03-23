using Logistics_Transportation.Models;

namespace Logistics_Transportation.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllWithFilterAsync(string? email, string? phone);
        Task<User?> GetByIdAsync(string id);

        Task DeleteAsync(User user);
    }
}
