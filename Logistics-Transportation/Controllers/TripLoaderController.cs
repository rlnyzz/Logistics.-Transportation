using Logistics_Transportation.DTOs;
using Logistics_Transportation.Models;
using Logistics_Transportation.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Logistics_Transportation.Controllers
{
    [ApiController]
    [Route("api/trip-loader")]
    public class TripLoaderController : ControllerBase
    {
        private readonly ITripLoaderRepository _tripLoaderRepository;
        public TripLoaderController(ITripLoaderRepository tripLoaderRepository)
        {
            _tripLoaderRepository = tripLoaderRepository;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllTripLoaders()
        {
            var tripLoader = await _tripLoaderRepository.GetAllAsync();
            return Ok(tripLoader);
        }

        [HttpGet("{id}")]
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
            return Ok(tripLoader);
        }

        [HttpPut("{id}")]
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
            return Ok("Рейс и грузчики успешно обновлены");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTripLoader(int id)
        {
            var tripLoader = await _tripLoaderRepository.GetByIdAsync(id);
            if (tripLoader == null)
            {
                return NotFound("Рейс и грузчики не найдены по данному id");
            }

            await _tripLoaderRepository.DeleteAsync(tripLoader);
            return Ok("Рейс и грузчики успешно удалены");
        }

    }
}
