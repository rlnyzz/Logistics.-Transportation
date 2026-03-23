using Logistics_Transportation.Models;
using Microsoft.EntityFrameworkCore;

namespace Logistics_Transportation.Repositories
{
    public class DriverRepository : IDriverRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public DriverRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Driver>> GetAllAsync()
        {
            return await _dbContext.Drivers.AsNoTracking().ToListAsync();
        }
        public async Task<Driver?> GetByIdAsync(int id)
        {
            var driver = await _dbContext.Drivers.FirstOrDefaultAsync(c => c.Id == id);
            return driver;
        }
        public async Task AddAsync(Driver driver)
        {
            await _dbContext.Drivers.AddAsync(driver);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateAsync(Driver driver)
        {
            _dbContext.Drivers.Update(driver);
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(Driver driver)
        {
            await _dbContext.Drivers.Where(c => c.Id == driver.Id).ExecuteDeleteAsync();
        }
        public async Task<LicenceCategories?> GetLicenceCategoryByNameAsync(string name)
        {
            var licence = await _dbContext.LicenceCategories.FirstOrDefaultAsync(c => c.Name == name);
            return licence;
        }

        public async Task<List<Driver>> GetAllWithFilterAsync(string? name, string? passport, 
            int? minAge, int? maxAge,
            int? minRate, int? maxRate,
            string? licenceCategory)
        {
            var query = _dbContext.Drivers.AsNoTracking().AsQueryable();
            var licence = await _dbContext.LicenceCategories.FirstOrDefaultAsync(c => c.Name == licenceCategory);

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

            if (minRate.HasValue)
            {
                query = query.Where(c => c.Rate >= minRate);
            }
            if (maxRate.HasValue)
            {
                query = query.Where(c => c.Rate <= maxRate);
            }

            if (!string.IsNullOrEmpty(licenceCategory)) 
            {
                query = query.Where(c => c.CategoryLicenceId == licence.Id);
            }

            return await query.ToListAsync();
        }
    }
}
