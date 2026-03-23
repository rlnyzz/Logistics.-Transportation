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
        public async Task<List<Car>> GetAllWithFilterAsync(string? carMake, string? carModel, string? typeOfCar, string? carNumber,
            decimal? minCargoCapacityT, decimal? maxCargoCapacityT,
            decimal? minTrunkVolumeL, decimal? maxTrunkVolumeL,
            decimal? minFuelConsumption, decimal? maxFuelConsumption,
            string? licenceCategory)
        {
            var query = _dbContext.Cars.AsNoTracking().AsQueryable();
            var licence = await _dbContext.LicenceCategories.FirstOrDefaultAsync(c => c.Name == licenceCategory);

            if (!string.IsNullOrEmpty(carMake))
            {
                query = query.Where(c => c.CarMake.Contains(carMake));
            }
            if (!string.IsNullOrEmpty(carModel))
            {
                query = query.Where(c => c.CarModel.Contains(carModel));
            }
            if (!string.IsNullOrEmpty(typeOfCar))
            {
                query = query.Where(c => c.TypeOfCar.Contains(typeOfCar));
            }
            if (!string.IsNullOrEmpty(carNumber))
            {
                query = query.Where(c => c.CarNumber.Contains(carNumber));
            }

            if (minCargoCapacityT.HasValue)
            {
                query = query.Where(c => c.CargoCapacityT >= minCargoCapacityT);
            }
            if (maxCargoCapacityT.HasValue)
            {
                query = query.Where(c => c.CargoCapacityT <= maxCargoCapacityT);
            }

            if (minTrunkVolumeL.HasValue)
            {
                query = query.Where(c => c.TrunkVolumeL >= minTrunkVolumeL);
            }
            if (maxTrunkVolumeL.HasValue)
            {
                query = query.Where(c => c.TrunkVolumeL <= maxTrunkVolumeL);
            }

            if (minFuelConsumption.HasValue)
            {
                query = query.Where(c => c.FuelConsumption >= minFuelConsumption);
            }
            if (maxFuelConsumption.HasValue)
            {
                query = query.Where(c => c.FuelConsumption <= maxFuelConsumption);
            }

            if (!string.IsNullOrEmpty(licenceCategory))
            {
                query = query.Where(c => c.LicenceCategoriesId == licence.Id);
            }
            
            return await query.ToListAsync(); 
        }
    }
}
