using Logistics_Transportation.DTOs;
using Logistics_Transportation.Models;
using Logistics_Transportation.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Logistics_Transportation.Controllers
{
    [ApiController]
    [Route("api/trip")]
    public class TripController : ControllerBase
    {
        private readonly ITripRepository _tripRepository;
        public TripController(ITripRepository tripRepository)
        {
            _tripRepository = tripRepository;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllTrips()
        {
            var trip = await _tripRepository.GetAllAsync();
            return Ok(trip);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTripById(int id)
        {
            var trip = await _tripRepository.GetByIdAsync(id);
            if (trip == null)
            {
                return NotFound("Рейс не найден по данному id");
            }
            return Ok(trip);
        }

        [HttpPost]
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
            return Ok(trip);
        }

        [HttpPut("{id}")]
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
            return Ok("Рейс успешно обновлен");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrip(int id)
        {
            var trip = await _tripRepository.GetByIdAsync(id);
            if (trip == null)
            {
                return NotFound("Рейс не найден по данному id");
            }

            await _tripRepository.DeleteAsync(trip);
            return Ok("Рейс успешно удален");
        }
    }
}
