using Logistics_Transportation.Migrations;
using Logistics_Transportation.Repositories;
using Logistics_Transportation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Logistics_Transportation.Models;

namespace Logistics_Transportation.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        string UserNameForAction { get; set; } = "User";

        private readonly IUserRepository _userRepository;
        private readonly IActionLogService _actionService;
        public UserController(IUserRepository userRepository, IActionLogService actionService)
        {
            _userRepository = userRepository;
            _actionService = actionService;
        }

        [HttpGet("all")]
        [Authorize(Roles = "Operator,Admin")]
        public async Task<IActionResult> GetAllUsers([FromQuery] string? email, [FromQuery] string? phone)
        {
            var user = await _userRepository.GetAllWithFilterAsync(email, phone);
            return Ok(user);
        }

        [HttpGet("client-profile")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> GetClientProfileById()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepository.GetByIdAsync(userId);
            return Ok(user);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Operator,Admin")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound("Пользователь не найден по данному id");
            }
            return Ok(user);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound("Пользователь не найден");

            await _userRepository.DeleteAsync(user);

            var actionLog = new Models.ActionLog
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Action = "DELETE",
                EntityName = UserNameForAction,
                CreatedTime = DateTime.UtcNow
            };

            await _actionService.LogAsync(actionLog);

            return Ok("Пользователь успешно удален");
        }
    }
}