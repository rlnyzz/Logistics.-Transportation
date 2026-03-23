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
    [Route("api/trip-loader")]
    public class TripLoaderController : ControllerBase
    {
        string TripLoaderNameForAction { get; set; } = "TripLoader";

        private readonly ITripLoaderRepository _tripLoaderRepository;
        private readonly IActionLogService _actionService;
        public TripLoaderController(ITripLoaderRepository tripLoaderRepository, IActionLogService actionService)
        {
            _tripLoaderRepository = tripLoaderRepository;
            _actionService = actionService;
        }

        [HttpGet("all")]
        [Authorize(Roles = "Operator,Admin")]
        public async Task<IActionResult> GetAllTripLoaders([FromQuery] int? tripId, [FromQuery] int? loaderId)
        {
            var tripLoader = await _tripLoaderRepository.GetAllWithFilterAsync(tripId, loaderId);
            return Ok(tripLoader);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Operator,Admin")]
        public async Task<IActionResult> GetTripLoadersById(int id)
        {
            var tripLoader = await _tripLoaderRepository.GetByIdAsync(id);
            if (tripLoader == null)
            {
                return NotFound("Грузчики для рейсов не найдены по данному id");
            }

            return Ok(tripLoader);
        }

        [HttpPost]
        [Authorize(Roles = "Operator,Admin")]
        public async Task<IActionResult> CreateTripLoader([FromBody] CreateTripLoaderDTO dto)
        {
            var trip = await _tripLoaderRepository.GetTripIdByIdAsync(dto.TripId);
            if (trip == null)
            {
                return NotFound("Рейс не найден по данному id");
            }

            var loader = await _tripLoaderRepository.GetLoaderIdByIdAsync(dto.LoaderId);
            if (loader == null)
            {
                return NotFound("Грузчик не найден по данному id");
            }

            var tripLoader = new TripLoaders
            {
                TripId = dto.TripId,
                LoaderId = dto.LoaderId
            };

            await _tripLoaderRepository.AddAsync(tripLoader);

            var actionLog = new ActionLog
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Action = "CREATE",
                EntityName = TripLoaderNameForAction,
                EntityId = tripLoader.Id,
                CreatedTime = DateTime.UtcNow
            };

            await _actionService.LogAsync(actionLog);

            return Ok(tripLoader);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Operator")]
        public async Task<IActionResult> PutTripLoader(int id, [FromBody] UpdateTripLoaderDTO dto)
        {
            var tripLoader = await _tripLoaderRepository.GetByIdAsync(id);
            if (tripLoader == null)
            {
                return NotFound("Рейсы и грузчики не найдены по данному id");
            }

            var trip = await _tripLoaderRepository.GetTripIdByIdAsync(dto.TripId);
            if(trip == null)
            {
                return NotFound("Рейс не найден по данному id");
            }

            var loader = await _tripLoaderRepository.GetLoaderIdByIdAsync(dto.LoaderId);
            if (loader == null)
            {
                return NotFound("Грузчик не найден по данному id");
            }

            tripLoader.TripId = dto.TripId;
            tripLoader.LoaderId = dto.LoaderId;

            await _tripLoaderRepository.UpdateAsync(tripLoader);

            var actionLog = new ActionLog
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Action = "PUT",
                EntityName = TripLoaderNameForAction,
                EntityId = tripLoader.Id,
                CreatedTime = DateTime.UtcNow
            };

            await _actionService.LogAsync(actionLog);

            return Ok("Рейс и грузчики успешно обновлены");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Operator")]
        public async Task<IActionResult> DeleteTripLoader(int id)
        {
            var tripLoader = await _tripLoaderRepository.GetByIdAsync(id);
            if (tripLoader == null)
            {
                return NotFound("Рейс и грузчики не найдены по данному id");
            }

            await _tripLoaderRepository.DeleteAsync(tripLoader);

            var actionLog = new ActionLog
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Action = "DELETE",
                EntityName = TripLoaderNameForAction,
                EntityId = tripLoader.Id,
                CreatedTime = DateTime.UtcNow
            };

            await _actionService.LogAsync(actionLog);

            return Ok("Рейс и грузчики успешно удалены");
        }

    }
}
