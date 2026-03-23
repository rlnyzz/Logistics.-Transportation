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
    [Route("api/loader")]
    public class LoaderController : ControllerBase
    {
        private readonly ILoaderRepository _loaderRepository;
        private readonly IActionLogService _actionService;
        public LoaderController(ILoaderRepository loaderRepository, IActionLogService actionService)
        {
            _loaderRepository = loaderRepository;
            _actionService = actionService;
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin,Operator")]
        public async Task<IActionResult> GetAllLoaders([FromQuery] string? name, [FromQuery] string? passport, [FromQuery] int? minAge, [FromQuery] int? maxAge)
        {
            var loader = await _loaderRepository.GetAllWithFilterAsync(name, passport, minAge, maxAge);
            return Ok(loader);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Operator")]
        public async Task<IActionResult> GetLoaderById(int id)
        {
            var loader = await _loaderRepository.GetByIdAsync(id);
            if (loader == null)
            {
                return NotFound("Грузчик не найден по данному id");
            }

            return Ok(loader);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateLoader([FromBody] CreateLoaderDTO dto)
        {
            var loader = new Loader
            {
                Name = dto.Name,
                Passport = dto.Passport,
                Age = dto.Age
            };
            await _loaderRepository.AddAsync(loader);

            var actionLog = new ActionLog
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Action = "CREATE",
                EntityName = "Loader",
                EntityId = loader.Id,
                CreatedTime = DateTime.UtcNow
            };

            await _actionService.LogAsync(actionLog);

            return Ok(loader);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutLoader(int id, [FromBody] UpdateLoaderDTO dto)
        {
            var loader = await _loaderRepository.GetByIdAsync(id);
            if (loader == null)
            {
                return NotFound("Грузчик не найден по данному id");
            }

            loader.Name = dto.Name;
            loader.Passport = dto.Passport;
            loader.Age = dto.Age;


            await _loaderRepository.UpdateAsync(loader);

            var actionLog = new ActionLog
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Action = "PUT",
                EntityName = "Loader",
                EntityId = loader.Id,
                CreatedTime = DateTime.UtcNow
            };

            await _actionService.LogAsync(actionLog);

            return Ok("Грузчик успешно обновлен");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteLoader(int id)
        {
            var loader = await _loaderRepository.GetByIdAsync(id);
            if (loader == null)
            {
                return NotFound("Грузчик не найден по данному id");
            }

            await _loaderRepository.DeleteAsync(loader);

            var actionLog = new ActionLog
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Action = "DELETE",
                EntityName = "Loader",
                EntityId = loader.Id,
                CreatedTime = DateTime.UtcNow
            };

            await _actionService.LogAsync(actionLog);

            return Ok("Грузчик успешно удален");
        }
    }
}
