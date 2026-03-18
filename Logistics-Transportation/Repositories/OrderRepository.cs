using Logistics_Transportation.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Order?> GetByIdAsync(int id)
        {
            var order = await _dbContext.Orders.FirstOrDefaultAsync(c => c.Id == id);
            return order;
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
    }
}
