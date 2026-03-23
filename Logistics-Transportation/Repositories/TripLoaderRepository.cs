using Logistics_Transportation.Models;
using Microsoft.EntityFrameworkCore;

namespace Logistics_Transportation.Repositories
{
    public class TripLoaderRepository : ITripLoaderRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public TripLoaderRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TripLoaders>> GetAllWithFilterAsync(int? tripId, int? loaderId)
        {
            var query = _dbContext.TripLoaders.AsNoTracking().AsQueryable();

            if (tripId.HasValue)
            {
                query = query.Where(c => c.TripId == tripId);
            }
            if (loaderId.HasValue)
            {
                query = query.Where(c => c.LoaderId == loaderId);
            }

            return await query.ToListAsync();
        }

        public async Task<TripLoaders?> GetByIdAsync(int id)
        {
            var tripLoader = await _dbContext.TripLoaders.FirstOrDefaultAsync(c => c.Id == id);
            return tripLoader;
        }
        public async Task AddAsync(TripLoaders tripLoader)
        {
            await _dbContext.TripLoaders.AddAsync(tripLoader);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateAsync(TripLoaders tripLoader)
        {
            _dbContext.TripLoaders.Update(tripLoader);
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(TripLoaders tripLoader)
        {
            await _dbContext.TripLoaders.Where(c => c.Id == tripLoader.Id).ExecuteDeleteAsync();
        }

        public async Task<Trip?> GetTripIdByIdAsync(int id)
        {
            var trip = await _dbContext.Trips.FirstOrDefaultAsync(c => c.Id == id);
            return trip;
        }
        public async Task<Loader?> GetLoaderIdByIdAsync(int id)
        {
            var loader = await _dbContext.Loaders.FirstOrDefaultAsync(c => c.Id == id);
            return loader;
        }
    }
}