using Logistics_Transportation.Models;
using Microsoft.EntityFrameworkCore;

namespace Logistics_Transportation.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CarRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Car>> GetAllAsync()
        {
            return await _dbContext.Cars.AsNoTracking().ToListAsync();
        }
        public async Task<Car?> GetByIdAsync(int id)
        {
            var car = await _dbContext.Cars.FirstOrDefaultAsync(c => c.Id == id);
            return car;
        }
        public async Task AddAsync(Car car)
        {
            await _dbContext.Cars.AddAsync(car);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateAsync(Car car)
        {
            _dbContext.Cars.Update(car);
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(Car car)
        {
            await _dbContext.Cars.Where(c => c.Id == car.Id).ExecuteDeleteAsync();
        }
        public async Task<LicenceCategories?> GetLicenceCategoryByNameAsync(string name)
        {
            var licence = await _dbContext.LicenceCategories.FirstOrDefaultAsync(c => c.Name == name);
            return licence;
        }
    }
}
