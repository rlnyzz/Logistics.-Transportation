using Logistics_Transportation.Models;
using Microsoft.EntityFrameworkCore;

namespace Logistics_Transportation.Repositories
{
    public class TripRepository : ITripRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public TripRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Trip>> GetAllAsync()
        {
            return await _dbContext.Trips.AsNoTracking().ToListAsync();
        }
        public async Task<Trip?> GetByIdAsync(int id)
        {
            var trip = await _dbContext.Trips.FirstOrDefaultAsync(c => c.Id == id);
            return trip;
        }
        public async Task<Trip?> GetOrderIdByIdAsync(int orderId)
        {
            var orderById = await _dbContext.Trips.FirstOrDefaultAsync(t => t.OrderId == orderId);
            return orderById;
        }
        public async Task<Order?> GetByIdAsyncOrder(int id)
        {
            var order = await _dbContext.Orders.FirstOrDefaultAsync(c => c.Id == id);
            return order;
        }
        public async Task<Driver?> GetByIdAsyncDriver(int id)
        {
            var driver = await _dbContext.Drivers.FirstOrDefaultAsync(c => c.Id == id);
            return driver;
        }
        public async Task<Car?> GetByIdAsyncCar(int id)
        {
            var car = await _dbContext.Cars.FirstOrDefaultAsync(c => c.Id == id);
            return car;
        }
        public async Task AddAsync(Trip trip)
        {
            await _dbContext.Trips.AddAsync(trip);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateAsync(Trip trip)
        {
            _dbContext.Trips.Update(trip);
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(Trip trip)
        {
            await _dbContext.Trips.Where(c => c.Id == trip.Id).ExecuteDeleteAsync();
        }
    }
}
