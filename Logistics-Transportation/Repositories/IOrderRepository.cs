using Logistics_Transportation.Models;

namespace Logistics_Transportation.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllAsync();
        Task<List<Order>> GetAllByUserIdWithFilterAsync(string userId, string? pickAppAdress, string? deliveryAdress, string? description, DateTime? dateFrom, DateTime? dateTo,
            double? minWeight, double? maxWeight, double? minVolume, double? maxVolume);
        Task<Order?> GetByIdAsync(int id);
        Task<Order?> GetOrderByUserIdAsync(string userId, int id);
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(Order order);

        Task<List<Order>> GetAllWithFilterAsync(string? email, string? pickAppAdress, string? deliveryAdress, string? description, DateTime? dateFrom, DateTime? dateTo,
            double? minWeight, double? maxWeight, double? minVolume, double? maxVolume);
    }
}
