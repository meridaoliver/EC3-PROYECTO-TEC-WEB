using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CoworkingReservations.Core.DTOs;
using CoworkingReservations.Core.Interfaces; // IUserService
using CoworkingReservations.Core.Shared; // ApiResponse

namespace CoworkingReservations.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public TokenController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Authentication([FromBody] UserLoginDto login)
        {
            var validation = await _userService.ValidateUser(login);
            if (validation.IsValid)
            {
                var token = GenerateToken(validation.User!);
                return Ok(new ApiResponse<string>(token) { Message = "Autenticación exitosa" });
            }

            return NotFound(new ApiResponse<string>(null) { Message = "Credenciales inválidas" });
        }

        private string GenerateToken(Core.Entities.User user)
        {
            // Header
            var symmetricSecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]!));
            var signingCredentials = new SigningCredentials(
                symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            // Payload
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("UserId", user.Id.ToString()) // Útil para filtrar datos
            };

            var payload = new JwtPayload(
                issuer: _configuration["Authentication:Issuer"],
                audience: _configuration["Authentication:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(120) // 2 horas
            );

            var token = new JwtSecurityToken(header, payload);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CoworkingReservations.Core.DTOs;
using CoworkingReservations.Core.Interfaces; // IUserService
using CoworkingReservations.Core.Shared; // ApiResponse

namespace CoworkingReservations.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public TokenController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Authentication([FromBody] UserLoginDto login)
        {
            var validation = await _userService.ValidateUser(login);
            if (validation.IsValid)
            {
                var token = GenerateToken(validation.User!);
                return Ok(new ApiResponse<string>(token) { Message = "Autenticación exitosa" });
            }

            return NotFound(new ApiResponse<string>(null) { Message = "Credenciales inválidas" });
        }

        private string GenerateToken(Core.Entities.User user)
        {
            // Header
            var symmetricSecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]!));
            var signingCredentials = new SigningCredentials(
                symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            // Payload
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("UserId", user.Id.ToString()) // Útil para filtrar datos
            };

            var payload = new JwtPayload(
                issuer: _configuration["Authentication:Issuer"],
                audience: _configuration["Authentication:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(120) // 2 horas
            );

            var token = new JwtSecurityToken(header, payload);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}