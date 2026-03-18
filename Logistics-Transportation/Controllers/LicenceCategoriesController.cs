using Logistics_Transportation.DTOs;
using Logistics_Transportation.Models;
using Logistics_Transportation.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Logistics_Transportation.Controllers
{
    [ApiController]
    [Route("api/licence-categories")]
    public class LicenceCategoriesController : ControllerBase
    {
        private readonly ILicenceCategoryRepository _licenceCategoryRepository;
        public LicenceCategoriesController(ILicenceCategoryRepository licenceCategoryRepository)
        {
            _licenceCategoryRepository = licenceCategoryRepository;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllLicenceCategories()
        {
            var licenceCategories = await _licenceCategoryRepository.GetAllAsync();
            return Ok(licenceCategories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLicenceCategoriesById(int id)
        {
            var licenceCategory = await _licenceCategoryRepository.GetByIdAsync(id);

            if(licenceCategory == null)
            {
                return NotFound("Категория прав не найдено по данному id");
            }

            return Ok(licenceCategory);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLicenceCategories([FromBody] CreateLicenceCategoriesDTO dto)
        {
            var licenceCategory = new LicenceCategories
            {
                Name = dto.Name
            };
            await _licenceCategoryRepository.AddAsync(licenceCategory);
            return Ok(licenceCategory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLicenceCategories(int id, [FromBody] UpdateLicenceCategoriesDTO dto)
        {
            var licenceCategory = await _licenceCategoryRepository.GetByIdAsync(id);

            if(licenceCategory == null)
            {
                return NotFound("Категория прав не найдено по данному id");
            }

            licenceCategory.Name = dto.Name;

            await _licenceCategoryRepository.UpdateAsync(licenceCategory);
            return Ok("Категория прав успешно обновлена");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLicenceCategories(int id)
        {
            var licenceCategory = await _licenceCategoryRepository.GetByIdAsync(id);

            if(licenceCategory == null)
            {
               return NotFound("Категория прав не найдено по данному id");
            }

            await _licenceCategoryRepository.DeleteAsync(licenceCategory);
            return Ok("Категория прав успешно удалена");
        }
    }
}
