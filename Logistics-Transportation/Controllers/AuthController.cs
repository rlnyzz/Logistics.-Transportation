using Logistics_Transportation.DTOs;
using Logistics_Transportation.Models;
using Logistics_Transportation.Security;
using Logistics_Transportation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
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

            Response.Cookies.Append("accessToken", accessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            var refreshToken = await _refreshToken.GenerateRefreshTokenAsync(user.Id);

            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(1)
            });

            return Ok("Ok");
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return Unauthorized();

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                id = user.Id,
                email = user.Email,
                phone = user.PhoneNumber,
                role = roles.FirstOrDefault()
            });
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

            Response.Cookies.Append("accessToken", accessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            await _refreshToken.RevokeAsync(dto.RefreshToken);

            var newRefreshToken = await _refreshToken.GenerateRefreshTokenAsync(user.Id);

            Response.Cookies.Append("refreshToken", newRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(1)
            });

            return Ok(new { accessToken, refreshToken = newRefreshToken });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (!string.IsNullOrEmpty(refreshToken))
                await _refreshToken.RevokeAsync(refreshToken);

            var cookieOptions = new CookieOptions
            {
                Secure = true,
                SameSite = SameSiteMode.None,
                HttpOnly = true
            };

            Response.Cookies.Delete("accessToken", cookieOptions);
            Response.Cookies.Delete("refreshToken", cookieOptions);

            return Ok();
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
