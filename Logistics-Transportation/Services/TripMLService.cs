using Logistics_Transportation.Models;
using Microsoft.EntityFrameworkCore;

namespace Logistics_Transportation.Services
{
    public class TripMLService : ITripMLService
    {
        private readonly ApplicationDbContext _dbContext;
        public TripMLService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Trip?> CreateTripWithMLFunctionAsync(Order order)
        {
            var inputML = new MLModel.ModelInput
            {
                Description = order.Description
            };

            var result = MLModel.Predict(inputML);
            var typeCar = result.PredictedLabel;

            var car = await _dbContext.Cars
                .Where(c => c.TypeOfCar == typeCar
                         && c.CargoCapacityT >= (decimal)order.CargoWeight / 1000
                         && c.TrunkVolumeL >= (decimal)order.CargoVolume)
                .OrderBy(c => c.CargoCapacityT)
                .ThenBy(c => c.TrunkVolumeL)
                .FirstOrDefaultAsync();
            if (car == null)
            {
                return null;
            }

            var driver = await _dbContext.Drivers.FirstOrDefaultAsync(c => c.CategoryLicenceId == car.LicenceCategoriesId);

            if (driver == null)
            {
                return null;
            }

            var trip = new Trip
            {
                OrderId = order.Id,
                CarId = car.Id,
                DriverId = driver.Id,
                FinalePrice = 0,
                FinaleTimeMinutes = 0
            };

            await _dbContext.Trips.AddAsync(trip);
            await _dbContext.SaveChangesAsync();

            return trip;
        }
    }
}
