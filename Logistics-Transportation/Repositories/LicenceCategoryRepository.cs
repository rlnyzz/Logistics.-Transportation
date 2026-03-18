using Logistics_Transportation.Models;
using Microsoft.EntityFrameworkCore;

namespace Logistics_Transportation.Repositories
{
    public class LicenceCategoryRepository : ILicenceCategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public LicenceCategoryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<LicenceCategories>> GetAllAsync()
        {
            return await _dbContext.LicenceCategories.AsNoTracking().ToListAsync();
        }
        public async Task<LicenceCategories?> GetByIdAsync(int id)
        {
            var licenceCategory = await _dbContext.LicenceCategories.FirstOrDefaultAsync(c => c.Id == id);
            return licenceCategory;
        }
        public async Task AddAsync(LicenceCategories licenceCategories)
        {
            await _dbContext.LicenceCategories.AddAsync(licenceCategories);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateAsync(LicenceCategories licenceCategories)
        {
            _dbContext.LicenceCategories.Update(licenceCategories);
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(LicenceCategories licenceCategories)
        {
            await _dbContext.LicenceCategories.Where(c => c.Id == licenceCategories.Id).ExecuteDeleteAsync();
        }
    }
}
