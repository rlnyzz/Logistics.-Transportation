using Logistics_Transportation.Models;

namespace Logistics_Transportation.Repositories
{
    public interface ILoaderRepository
    {
        Task<List<Loader>> GetAllAsync();
        Task<Loader?> GetByIdAsync(int id);
        Task AddAsync(Loader loader);
        Task UpdateAsync(Loader loader);
        Task DeleteAsync(Loader loader);
    }
}
