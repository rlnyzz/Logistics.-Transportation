    using Logistics_Transportation.DTOs;
    using Logistics_Transportation.Models;
    using Logistics_Transportation.Repositories;
using Logistics_Transportation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Logistics_Transportation.Controllers
{
    [ApiController]
    [Route("api/car")]
    public class CarController : ControllerBase
    {
        private readonly ICarRepository _carRepository;
        private readonly IActionLogService _actionLogService;
        public CarController(IActionLogService actionLogService, ICarRepository carRepository)
        {
            _actionLogService = actionLogService;
            _carRepository = carRepository;
        }

    [HttpGet("all")]
    [Authorize(Roles = "Admin,Operator")]
    public async Task<IActionResult> GetAllCars([FromQuery] string? carMake, [FromQuery] string? carModel,[FromQuery] string? TypeOfCar, [FromQuery] string? carNumber, 
            [FromQuery] decimal? minCargoCapacityT, [FromQuery] decimal? maxCargoCapacityT,
            [FromQuery] decimal? minTrunkVolumeT, [FromQuery] decimal? maxTrunkVolumeL, 
            [FromQuery] decimal? minFuelConsumption, [FromQuery] decimal? maxFuelConsumption,
            [FromQuery] string? licenceCategory)
        {
            var car = await _carRepository.GetAllWithFilterAsync(carMake, carModel, TypeOfCar, carNumber, 
                minCargoCapacityT, maxCargoCapacityT, 
                minTrunkVolumeT, maxTrunkVolumeL,
                minFuelConsumption, maxFuelConsumption,
                licenceCategory);
            return Ok(car);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Operator")]
        public async Task<IActionResult> GetCarsById(int id)
        {
            var car = await _carRepository.GetByIdAsync(id);
            if (car == null)
            {
                return NotFound("Машина не найдена по данному id");
            }
            return Ok(car);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCar([FromBody] CreateCarDTO dto)
        {
            var licenceCategory = await _carRepository.GetLicenceCategoryByNameAsync(dto.LicenceCategories);
            if (licenceCategory == null)
            {
                return NotFound("Категория прав не найдена");
            }

            var car = new Car
            {
                CarMake = dto.CarMake,
                CarModel = dto.CarModel,
                TypeOfCar = dto.TypeOfCar,
                CarNumber = dto.CarNumber,
                CargoCapacityT = dto.CargoCapacityT,
                TrunkVolumeL = dto.TrunkVolumeL,
                FuelConsumption = dto.FuelConsumption,
                LicenceCategoriesId = licenceCategory.Id
            };

            await _carRepository.AddAsync(car);

            var actionLog = new ActionLog
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Action = "CREATE",
                EntityName = "Car",
                EntityId = car.Id,
                CreatedTime = DateTime.UtcNow
            };

            await _actionLogService.LogAsync(actionLog);
            return Ok(car);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutCar(int id, [FromBody] UpdateCarDTO dto)
        {
            var car = await _carRepository.GetByIdAsync(id);
            if (car == null)
            {
                return NotFound("Машина не найдена по данному id");
            }
            var licenceCategory = await _carRepository.GetLicenceCategoryByNameAsync(dto.LicenceCategories);
            if (licenceCategory == null)
            {
                return NotFound("Категория прав не найдена");
            }

            car.CarMake = dto.CarMake;
            car.CarModel = dto.CarModel;
            car.TypeOfCar = dto.TypeOfCar;
            car.CarNumber = dto.CarNumber;
            car.CargoCapacityT = dto.CargoCapacityT;
            car.TrunkVolumeL = dto.TrunkVolumeL;
            car.FuelConsumption = dto.FuelConsumption;
            car.LicenceCategoriesId = licenceCategory.Id;

            await _carRepository.UpdateAsync(car);

            var actionLog = new ActionLog
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Action = "PUT",
                EntityName = "Car",
                EntityId = car.Id,
                CreatedTime = DateTime.UtcNow
            };

            await _actionLogService.LogAsync(actionLog);

            return Ok("Машина успешно обновлена");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DelateCar(int id)
        {
            var car = await _carRepository.GetByIdAsync(id);
            if(car == null)
            {
                return NotFound("Машина не найдена по данному id");
            }

            var actionLog = new ActionLog
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Action = "DELETE",
                EntityName = "Car",
                EntityId = car.Id,
                CreatedTime = DateTime.UtcNow
            };

            await _carRepository.DeleteAsync(car);

            await _actionLogService.LogAsync(actionLog);

            return Ok("Машина успешно удалена");
        }
    }
}

