using Logistics_Transportation.Models;

namespace Logistics_Transportation.Repositories
{
    public interface ITripLoaderRepository
    {
        Task<List<TripLoaders>> GetAllWithFilterAsync(int? tripId, int? loaderId);
        Task<TripLoaders?> GetByIdAsync(int id);
        Task AddAsync(TripLoaders tripLoaders);
        Task UpdateAsync(TripLoaders tripLoaders);
        Task DeleteAsync(TripLoaders tripLoaders);
        Task<Trip?> GetTripIdByIdAsync(int id);
        Task<Loader?> GetLoaderIdByIdAsync(int id);
    }
}
