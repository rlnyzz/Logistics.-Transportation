using Logistics_Transportation.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Logistics_Transportation.Models;

namespace Logistics_Transportation.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public AuthController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromQuery] string email, [FromQuery] string password, [FromQuery] string phone)
        {
            var user = new User { UserName = email, Email = email, PhoneNumber = phone};
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok("Пользователь успешно зарегистрирован");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromQuery] string email, [FromQuery] string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Unauthorized("Пользователь не найден");
            }
            var passwordCorrect = await _userManager.CheckPasswordAsync(user, password);
            if (!passwordCorrect)
            {
                return Unauthorized("Неверный пароль");
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromHours(2)),
                signingCredentials: new SigningCredentials(
                    AuthOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));

            return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
        }
    }
}
