using Logistics_Transportation.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Logistics_Transportation.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public OrderRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _dbContext.Orders.AsNoTracking().ToListAsync();
        }

        public async Task<List<Order>> GetAllByUserIdWithFilterAsync(string userId, string? pickAppAdress, string? deliveryAdress, string? description, DateTime? dateFrom, DateTime? dateTo,
            double? minWeight, double? maxWeight, double? minVolume, double? maxVolume)
        {
            var query = _dbContext.Orders.AsNoTracking().Where(c => c.UserId == userId).AsQueryable();

            if (!string.IsNullOrEmpty(pickAppAdress))
            {
                query = query.Where(c => c.PickAppAddress.Contains(pickAppAdress));
            }
            if (!string.IsNullOrEmpty(deliveryAdress))
            {
                query = query.Where(c => c.DeliveryAddress.Contains(deliveryAdress));
            }
            if (!string.IsNullOrEmpty(description))
            {
                query = query.Where(c => c.Description.Contains(description));
            }
            if (dateFrom.HasValue)
            {
                var from = DateTime.SpecifyKind(dateFrom.Value, DateTimeKind.Utc);
                query = query.Where(c => c.RegistrationDateOrder >= from);
            }
            if (dateTo.HasValue)
            {
                var to = DateTime.SpecifyKind(dateTo.Value, DateTimeKind.Utc);
                query = query.Where(c => c.RegistrationDateOrder <= to);
            }
            if (minWeight.HasValue)
            {
                query = query.Where(c => c.CargoWeight >= minWeight);
            }
            if (maxWeight.HasValue)
            {
                query = query.Where(c => c.CargoWeight <= maxWeight);
            }
            if (minVolume.HasValue)
            {
                query = query.Where(c => c.CargoVolume >= minVolume);
            }
            if (maxVolume.HasValue)
            {
                query = query.Where(c => c.CargoVolume <= maxVolume);
            }

            return await query.ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            var order = await _dbContext.Orders.FirstOrDefaultAsync(c => c.Id == id);
            return order;
        }

        public async Task<Order?> GetOrderByUserIdAsync(string userId, int id)
        {
            return await _dbContext.Orders
                .AsNoTracking().Where(c => c.UserId == userId)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task AddAsync(Order order)
        {
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateAsync(Order order)
        { 
            _dbContext.Orders.Update(order);
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(Order order)
        {
            await _dbContext.Orders.Where(c => c.Id == order.Id).ExecuteDeleteAsync();
        }
        public async Task<List<Order>> GetAllWithFilterAsync(string? email, string? pickAppAdress, string? deliveryAdress, string? description, DateTime? dateFrom, DateTime? dateTo,
            double? minWeight, double? maxWeight, double? minVolume, double? maxVolume)
        {
            var query = _dbContext.Orders.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(email))
            {
                var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(c => c.UserName == email);

                if (user != null)
                {
                    query = query.Where(c => c.UserId == user.Id);
                }
            }
            if (!string.IsNullOrEmpty(pickAppAdress))
            {
                query = query.Where(c => c.PickAppAddress.Contains(pickAppAdress));
            }
            if (!string.IsNullOrEmpty(deliveryAdress))
            {
                query = query.Where(c => c.DeliveryAddress.Contains(deliveryAdress));
            }
            if (!string.IsNullOrEmpty(description))
            {
                query = query.Where(c => c.Description.Contains(description));
            }
            if (dateFrom.HasValue)
            {
                var from = DateTime.SpecifyKind(dateFrom.Value, DateTimeKind.Utc);
                query = query.Where(c => c.RegistrationDateOrder >= from);
            }
            if (dateTo.HasValue)
            {
                var to = DateTime.SpecifyKind(dateTo.Value, DateTimeKind.Utc);
                query = query.Where(c => c.RegistrationDateOrder <= to);
            }
            if (minWeight.HasValue)
            {
                query = query.Where(c => c.CargoWeight >= minWeight);
            }
            if (maxWeight.HasValue)
            {
                query = query.Where(c => c.CargoWeight <= maxWeight);
            }
            if (minVolume.HasValue)
            {
                query = query.Where(c => c.CargoVolume >= minVolume);
            }
            if (maxVolume.HasValue)
            {
                query = query.Where(c => c.CargoVolume <= maxVolume);
            }
            
            return await query.ToListAsync();
        }
    }
}
