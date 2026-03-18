using Logistics_Transportation.DTOs;
using Logistics_Transportation.Models;
using Logistics_Transportation.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Logistics_Transportation.Controllers
{
    [ApiController]
    [Route("api/driver")]
    public class DriverController : ControllerBase
    {
        private readonly IDriverRepository _driverRepository;
        public DriverController(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllDrivers()
        {
            var driver = await _driverRepository.GetAllAsync();
            return Ok(driver);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDriverById(int id)
        {
            var driver = await _driverRepository.GetByIdAsync(id);
            if (driver == null)
            {
                return NotFound("Водитель не найден с данным id");
            }
            return Ok(driver);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDriver([FromBody] CreateDriverDTO dto)
        {
            var licenceCategory = await _driverRepository.GetLicenceCategoryByNameAsync(dto.LicenceCategories);
            if (licenceCategory == null)
            {
                return NotFound("Категория прав не найдена");
            }
            var driver = new Driver
            {
                Name = dto.Name,
                Passport = dto.Passport,
                Age = dto.Age,
                Rate = dto.Rate,
                CategoryLicenceId = licenceCategory.Id
            };
            await _driverRepository.AddAsync(driver);
            return Ok(driver);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDriver(int id, [FromBody] UpdateDriverDTO dto)
        {
            var driver = await _driverRepository.GetByIdAsync(id);
            if (driver == null)
            {
                return NotFound("Водитель не найден с данным id");
            }

            var licenceCategory = await _driverRepository.GetLicenceCategoryByNameAsync(dto.LicenceCategories);
            if (licenceCategory == null)
            {
                return NotFound("Категория прав не найдена");
            }
            driver.Name = dto.Name;
            driver.Passport = dto.Passport;
            driver.Age = dto.Age;
            driver.Rate = dto.Rate;
            driver.CategoryLicenceId = licenceCategory.Id;

            await _driverRepository.UpdateAsync(driver);
            return Ok("Водитель успешно обновлен");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriver(int id) 
        {
            var driver = await _driverRepository.GetByIdAsync(id);
            if (driver == null)
            {
                return NotFound("Водитель не найден с данным id");
            }
            await _driverRepository.DeleteAsync(driver);
            return Ok("Водитель успешно удален");
        }
    }
}
