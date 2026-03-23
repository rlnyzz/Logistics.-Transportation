using Logistics_Transportation.Models;
using Microsoft.EntityFrameworkCore;

namespace Logistics_Transportation.Repositories
{
    public class LoaderRepository : ILoaderRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public LoaderRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Loader>> GetAllWithFilterAsync(string? name, string? passport, int? minAge, int? maxAge)
        {
            var query = _dbContext.Loaders.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(c => c.Name.Contains(name));
            }
            if (!string.IsNullOrEmpty(passport))
            {
                query = query.Where(c => c.Passport.Contains(passport));
            }

            if (minAge.HasValue)
            {
                query = query.Where(c => c.Age >= minAge);
            }
            if (maxAge.HasValue)
            {
                query = query.Where(c => c.Age <= maxAge);
            }

            return await query.ToListAsync();
        }
        public async Task<Loader?> GetByIdAsync(int id)
        {
            var loader = await _dbContext.Loaders.FirstOrDefaultAsync(c => c.Id == id);
            return loader;
        }
        public async Task AddAsync(Loader loader)
        {
            await _dbContext.Loaders.AddAsync(loader);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateAsync(Loader loader)
        {
            _dbContext.Loaders.Update(loader);
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(Loader loader)
        {
            await _dbContext.Loaders.Where(c => c.Id == loader.Id).ExecuteDeleteAsync();
        }
    }
}
