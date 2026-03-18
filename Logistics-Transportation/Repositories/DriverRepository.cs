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
    }
}
