    using Logistics_Transportation.DTOs;
    using Logistics_Transportation.Models;
    using Logistics_Transportation.Repositories;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    namespace Logistics_Transportation.Controllers
    {
        [ApiController]
        [Route("api/car")]
        public class CarController : ControllerBase
        {
            private readonly ICarRepository _carRepository;
            public CarController(ICarRepository carRepository)
            {
                _carRepository = carRepository;
            }

            [HttpGet("all")]
            public async Task<IActionResult> GetAllCars()
            {
                var car = await _carRepository.GetAllAsync();
                return Ok(car);
            }

            [HttpGet("{id}")]
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
                return Ok(car);
            }

            [HttpPut("{id}")]
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
                return Ok("Машина успешно обновлена");
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DelateCar(int id)
            {
                var car = await _carRepository.GetByIdAsync(id);
                if(car == null)
                {
                    return NotFound("Машина не найдена по данному id");
                }
                
                await _carRepository.DeleteAsync(car);
                return Ok("Машина успешно удалена");
            }
        }
    }
