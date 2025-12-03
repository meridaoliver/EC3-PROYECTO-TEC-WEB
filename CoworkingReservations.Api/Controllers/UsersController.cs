using Microsoft.AspNetCore.Mvc;
using CoworkingReservations.Core.DTOs;       // Para UserCreateDto
using CoworkingReservations.Core.Interfaces; // Para IUserService
using CoworkingReservations.Core.Shared;     // Para ApiResponse (ajusta si lo tienes en otro lado)

namespace CoworkingReservations.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // 1. Declaramos la variable del servicio
        private readonly IUserService _userService;

        // 2. Inyectamos el servicio en el Constructor
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // 3. Este es el método que me preguntaste
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserCreateDto dto)
        {
            // Llamamos al servicio. Él se encarga de validar, hashear el password y guardar.
            await _userService.RegisterUser(dto);

            // Retornamos 201 Created con un mensaje estándar
            var response = new ApiResponse<string>("Usuario registrado exitosamente");
            return StatusCode(201, response);
        }
    }
}