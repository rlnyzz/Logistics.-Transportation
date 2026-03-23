using Logistics_Transportation.Models;

namespace Logistics_Transportation.Repositories
{
    public interface ITripRepository
    {
        Task<List<Trip>> GetAllWithFilterAsync(int? orderId, int? driverId, int? carId,
            decimal? minFinalePrice, decimal? maxFinalePrice,
            int? minFinaleTimeMinutes, int? maxFinaleTimeMinutes);
        Task<Trip?> GetByIdAsync(int id);
        Task<Trip?> GetOrderIdByIdAsync(int orderId);
        Task<Order?> GetByIdAsyncOrder(int id);
        Task<Driver?> GetByIdAsyncDriver(int id);
        Task<Car?> GetByIdAsyncCar(int id);
        Task AddAsync(Trip trip);
        Task UpdateAsync(Trip trip);
        Task DeleteAsync(Trip trip);

        Task<List<Trip>> GetAllByUserIdWithFilterAsync(string userId, int? orderId, int? driverId, int? carId,
            decimal? minFinalePrice, decimal? maxFinalePrice,
            int? minFinaleTimeMinutes, int? maxFinaleTimeMinutes);
        Task<Trip?> GetTripByUserIdAsync(string userId, int id);
    }
}
