using Logistics_Transportation.DTOs;
using Logistics_Transportation.Models;
using Logistics_Transportation.Security;
using Logistics_Transportation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Logistics_Transportation.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ICashRefreshTokenService _refreshToken;

        public AuthController(UserManager<User> userManager, ICashRefreshTokenService refreshToken)
        {
            _userManager = userManager;
            _refreshToken = refreshToken;
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

            await _userManager.AddToRoleAsync(user, "Client");

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

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromHours(2)),
                signingCredentials: new SigningCredentials(
                    AuthOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);

            var refreshToken = await _refreshToken.GenerateRefreshTokenAsync(user.Id);

            return Ok(new { accessToken, refreshToken });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] ValidateRefreshDTO dto)
        {
            var userId = await _refreshToken.ValidateRefreshTokenAsync(dto.RefreshToken);
            if (userId == null)
            {
                return Unauthorized("Недействительный refresh token");
            }

            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromHours(2)),
                signingCredentials: new SigningCredentials(
                    AuthOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);

            await _refreshToken.RevokeAsync(dto.RefreshToken);

            var newRefreshToken = await _refreshToken.GenerateRefreshTokenAsync(user.Id);

            return Ok(new { accessToken, refreshToken = newRefreshToken });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] ValidateRefreshDTO dto)
        {
            await _refreshToken.RevokeAsync(dto.RefreshToken);
            return Ok("Пользователь успешно вышел из системы");
        }

        [HttpPost("assign-role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("Пользователь не найден");

            var result = await _userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok($"Роль {role} успешно назначена");
        }
    }
}
