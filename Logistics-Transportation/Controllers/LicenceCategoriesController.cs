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
    [Route("api/licence-categories")]
    public class LicenceCategoriesController : ControllerBase
    {
        private readonly ILicenceCategoryRepository _licenceCategoryRepository;
        private readonly IActionLogService _actionService;
        public LicenceCategoriesController(ILicenceCategoryRepository licenceCategoryRepository, IActionLogService actionLogService)
        {
            _licenceCategoryRepository = licenceCategoryRepository;
            _actionService = actionLogService;
        }
 
        [HttpGet("all")]
        [Authorize(Roles = "Admin,Operator")]
        public async Task<IActionResult> GetAllLicenceCategories([FromQuery] string? licenceName)
        {
            var licenceCategories = await _licenceCategoryRepository.GetAllWithFilterAsync(licenceName);
            return Ok(licenceCategories);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Operator")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateLicenceCategories([FromBody] CreateLicenceCategoriesDTO dto)
        {
            var licenceCategory = new LicenceCategories
            {
                Name = dto.Name
            };

            await _licenceCategoryRepository.AddAsync(licenceCategory);

            var actionLog = new ActionLog
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Action = "CREATE",
                EntityName = "LicenceCategory",
                EntityId = licenceCategory.Id,
                CreatedTime = DateTime.UtcNow
            };

            await _actionService.LogAsync(actionLog);

            return Ok(licenceCategory);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateLicenceCategories(int id, [FromBody] UpdateLicenceCategoriesDTO dto)
        {
            var licenceCategory = await _licenceCategoryRepository.GetByIdAsync(id);

            if(licenceCategory == null)
            {
                return NotFound("Категория прав не найдено по данному id");
            }

            licenceCategory.Name = dto.Name;

            await _licenceCategoryRepository.UpdateAsync(licenceCategory);

            var actionLog = new ActionLog
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Action = "PUT",
                EntityName = "LicenceCategory",
                EntityId = licenceCategory.Id,
                CreatedTime = DateTime.UtcNow
            };

            await _actionService.LogAsync(actionLog);

            return Ok("Категория прав успешно обновлена");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteLicenceCategories(int id)
        {
            var licenceCategory = await _licenceCategoryRepository.GetByIdAsync(id);

            if(licenceCategory == null)
            {
               return NotFound("Категория прав не найдено по данному id");
            }

            await _licenceCategoryRepository.DeleteAsync(licenceCategory);

            var actionLog = new ActionLog
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Action = "DELETE",
                EntityName = "LicenceCategory",
                EntityId = licenceCategory.Id,
                CreatedTime = DateTime.UtcNow
            };

            await _actionService.LogAsync(actionLog);

            return Ok("Категория прав успешно удалена");
        }
    }
}
