using Logistics_Transportation.Models;

namespace Logistics_Transportation.Repositories
{
    public interface ICarRepository
    {
        Task<List<Car>> GetAllAsync();
        Task<Car?> GetByIdAsync(int id);
        Task AddAsync(Car car);
        Task UpdateAsync(Car car);
        Task DeleteAsync(Car car);
        Task<LicenceCategories?> GetLicenceCategoryByNameAsync(string name);

        Task<List<Car>> GetAllWithFilterAsync(string? carMake, string? carModel, string? typeOfCar, string? carNumber,
            decimal? minCargoCapacityT, decimal? maxCargocapacity,
            decimal? minTrunkVolumeL, decimal? maxCargoCapacity, 
            decimal? minFuelConsumption, decimal? maxFuelConsumption,
            string? licenceCategory);
    }
}
