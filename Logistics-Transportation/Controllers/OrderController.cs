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

        [HttpGet("all-orders")]
        [Authorize(Roles = "Operator,Admin")]
        public async Task<IActionResult> GetAllOrders([FromQuery] string? email ,[FromQuery] string? pickAppAdress, [FromQuery] string? deliveryAdress, [FromQuery] string? description,[FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo,
            [FromQuery] double? minWeight, [FromQuery] double? maxWeight, [FromQuery] double? minVolume, [FromQuery] double? maxVolume)
        {
            var orders = await _orderRepository.GetAllWithFilterAsync(email, pickAppAdress, deliveryAdress, description,dateFrom, dateTo, minWeight, maxWeight, minVolume, maxVolume);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Operator,Admin")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound("Заказ не найден по данному id");
            }

            return Ok(order);
        }

        [HttpGet("all-client-orders")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> GetAllClientOrder([FromQuery] string? pickAppAdress, [FromQuery] string? deliveryAdress, [FromQuery] string? description, [FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo,
            [FromQuery] double? minWeight, [FromQuery] double? maxWeight, [FromQuery] double? minVolume, [FromQuery] double? maxVolume)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var orders = await _orderRepository.GetAllByUserIdWithFilterAsync(userId, pickAppAdress, deliveryAdress, description, dateFrom, dateTo, minWeight, maxWeight, minVolume, maxVolume);

            return Ok(orders);
            
        }

        [HttpGet("client-order-{id}")]
        [Authorize(Roles ="Client")]
        public async Task<IActionResult> GetClientOrderById(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var order = await _orderRepository.GetOrderByUserIdAsync(userId, id);
            if (order == null)
            {
                return NotFound("Заказ не найден");
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
        [Authorize(Roles = "Operator,Admin")]
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
        [Authorize(Roles = "Operator,Admin")]
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
