using Logistics_Transportation.Models;

namespace Logistics_Transportation.Repositories
{
    public interface ILoaderRepository
    {
        Task<List<Loader>> GetAllWithFilterAsync(string? name, string? passport, int? minAge, int? maxAge);
        Task<Loader?> GetByIdAsync(int id);
        Task AddAsync(Loader loader);
        Task UpdateAsync(Loader loader);
        Task DeleteAsync(Loader loader);
    }
}
