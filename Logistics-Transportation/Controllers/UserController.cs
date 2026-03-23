using Logistics_Transportation.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Logistics_Transportation.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
            return Ok("Пользователь успешно удален");
        }
    }
}