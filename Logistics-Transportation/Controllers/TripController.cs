using Logistics_Transportation.DTOs;
using Logistics_Transportation.Models;
using Logistics_Transportation.Repositories;
using Logistics_Transportation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Logistics_Transportation.Controllers
{
    [ApiController]
    [Route("api/trip")]
    public class TripController : ControllerBase
    {
        private readonly ITripRepository _tripRepository;
        private readonly IActionLogService _actionService;
        public TripController(ITripRepository tripRepository, IActionLogService actionService)
        {
            _tripRepository = tripRepository;
            _actionService = actionService;
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin,Operator")]
        public async Task<IActionResult> GetAllTrips([FromQuery] int? orderId, [FromQuery] int? driverId, [FromQuery] int? carId,
            [FromQuery] decimal? minFinalePrice, [FromQuery] decimal? maxFinalePrice,
            [FromQuery] int? minFinaleTimeMinutes, [FromQuery] int? maxFinaleTimeMinutes)
        {
            var trip = await _tripRepository.GetAllWithFilterAsync(orderId, driverId, carId, 
                minFinalePrice, maxFinalePrice,
                minFinaleTimeMinutes, maxFinaleTimeMinutes);
            return Ok(trip);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Operator")]
        public async Task<IActionResult> GetTripById(int id)
        {
            var trip = await _tripRepository.GetByIdAsync(id);
            if (trip == null)
            {
                return NotFound("Рейс не найден по данному id");
            }
            return Ok(trip);
        }

        [HttpGet("all-client-trips")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> GetAllClientTrip([FromQuery] int? orderId, [FromQuery] int? driverId, [FromQuery] int? carId,
            [FromQuery] decimal? minFinalePrice, [FromQuery] decimal? maxFinalePrice,
            [FromQuery] int? minFinaleTimeMinutes, [FromQuery] int? maxFinaleTimeMinutes)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var trips = await _tripRepository.GetAllByUserIdWithFilterAsync(userId, orderId, driverId, carId,
                minFinalePrice, maxFinalePrice,
                minFinaleTimeMinutes, maxFinaleTimeMinutes);

            return Ok(trips);
        }

        [HttpGet("client-trip-{id}")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> GetAllClientById(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var trip = await _tripRepository.GetTripByUserIdAsync(userId, id);
            if (trip == null)
            {
                return NotFound("Рейс не найден");
            }

            return Ok(trip);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Operator")]
        public async Task<IActionResult> CreateTrip([FromBody] CreateTripDTO dto)
        {
            var order = await _tripRepository.GetByIdAsyncOrder(dto.OrderId);
            if (order == null)
            {
                return NotFound("Заказ не найден");
            }

            var OrderInTrip = await _tripRepository.GetOrderIdByIdAsync(dto.OrderId);
            if (OrderInTrip != null)
            {
                return BadRequest("Заказ занят");
            }

            var driver = await _tripRepository.GetByIdAsyncDriver(dto.DriverId);
            if (driver == null)
            {
                return NotFound("Водитель не найден");
            }
            var car = await _tripRepository.GetByIdAsyncCar(dto.CarId);
            if (car == null)
            {
                return NotFound("Машина не найдена");
            }
            var trip = new Trip
            {
                OrderId = dto.OrderId,
                DriverId = dto.DriverId,
                CarId = dto.CarId,
                FinalePrice = dto.FinalePrice,
                FinaleTimeMinutes = dto.FinalTimeMinutes
            };

            await _tripRepository.AddAsync(trip);

            var actionLog = new ActionLog
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Action = "CREATE",
                EntityName = "Trip",
                EntityId = trip.Id,
                CreatedTime = DateTime.UtcNow
            };

            await _actionService.LogAsync(actionLog);

            return Ok(trip);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Operator")]
        public async Task<IActionResult> PutTrip(int id, [FromBody] UpdateTripDTO dto)
        {
            var trip = await _tripRepository.GetByIdAsync(id);
            if(trip == null)
            {
                return NotFound("Рейс не найден по данному id");
            }

            var driver = await _tripRepository.GetByIdAsyncDriver(dto.DriverId);
            if (driver == null)
            {
                return NotFound("Водитель не найдена по данному id");
            }

            var car = await _tripRepository.GetByIdAsyncCar(dto.CarId);
            if (car == null)
            {
                return NotFound("Машина не найдена по данному id");
            }

            trip.DriverId = dto.DriverId;
            trip.CarId = dto.CarId;
            trip.FinalePrice = dto.FinalePrice;
            trip.FinaleTimeMinutes = dto.FinalTimeMinutes;

            await _tripRepository.UpdateAsync(trip);

            var actionLog = new ActionLog
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Action = "PUT",
                EntityName = "Trip",
                EntityId = trip.Id,
                CreatedTime = DateTime.UtcNow
            };

            await _actionService.LogAsync(actionLog);

            return Ok("Рейс успешно обновлен");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Operator")]
        public async Task<IActionResult> DeleteTrip(int id)
        {
            var trip = await _tripRepository.GetByIdAsync(id);
            if (trip == null)
            {
                return NotFound("Рейс не найден по данному id");
            }

            await _tripRepository.DeleteAsync(trip);

            var actionLog = new ActionLog
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Action = "DELETE",
                EntityName = "Trip",
                EntityId = trip.Id,
                CreatedTime = DateTime.UtcNow
            };

            await _actionService.LogAsync(actionLog);

            return Ok("Рейс успешно удален");
        }
    }
}
