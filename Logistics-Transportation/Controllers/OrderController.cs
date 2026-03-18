using Logistics_Transportation.DTOs;
using Logistics_Transportation.Models;
using Logistics_Transportation.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Logistics_Transportation.Controllers
{
    [ApiController]
    [Route("api/order")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound("Заказ не найден по данному id");
            }

            return Ok(order);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDTO dto)
        {
            var order = new Models.Order
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                PickAppAddress = dto.PickAppAddress,
                DeliveryAddress = dto.DeliveryAddress,
                Description = dto.Description,
                CargoWeight = dto.CargoWeight,
                CargoVolume = dto.CargoVolume,
                RegistrationDateOrder = DateTime.UtcNow
            };
            await _orderRepository.AddAsync(order);
            return Ok(order);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutOrder(int id, [FromBody] UpdateOrderDTO dto)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound("Заказ не найден по данному id");
            }
            order.PickAppAddress = dto.PickAppAddress;
            order.DeliveryAddress = dto.DeliveryAddress;
            order.Description = dto.Description;
            order.CargoWeight = dto.CargoWeight;
            order.CargoVolume = dto.CargoVolume;

            await _orderRepository.UpdateAsync(order);
            return Ok("Заказ успешно обновлен");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound("Заказ не найден по данному id");
            }
            await _orderRepository.DeleteAsync(order);
            return Ok("Заказ успешно удален");
        }
    }
}
