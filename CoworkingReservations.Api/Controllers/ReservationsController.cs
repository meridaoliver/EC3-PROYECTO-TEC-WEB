using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using CoworkingReservations.Core.Interfaces;
using CoworkingReservations.Core.DTOs;
using CoworkingReservations.Core.Shared;

namespace CoworkingReservations.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _service;
        private readonly IMapper _mapper;

        public ReservationsController(IReservationService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReservationCreateDto dto)
        {
            // FluentValidation se ejecuta antes si está configurado
            try
            {
                var result = await _service.CreateReservationAsync(dto);
                if (result == null)
                {
                    var conflict = new ApiResponse<object> { Success = false, Message = "Conflict: el espacio ya está reservado en ese intervalo.", StatusCode = 409 };
                    return Conflict(conflict);
                }

                var resDto = _mapper.Map<ReservationDto>(result);
                var response = new ApiResponse<ReservationDto> { Success = true, Data = resDto, Message = "Reserva creada", StatusCode = StatusCodes.Status201Created };
                return CreatedAtAction(nameof(GetById), new { id = resDto.Id }, response);
            }
            catch (KeyNotFoundException knf)
            {
                return NotFound(new ApiResponse<object> { Success = false, Message = knf.Message, StatusCode = 404 });
            }
            catch (ArgumentException aex)
            {
                return BadRequest(new ApiResponse<object> { Success = false, Message = aex.Message, StatusCode = 400 });
            }
            catch (InvalidOperationException ioex)
            {
                return BadRequest(new ApiResponse<object> { Success = false, Message = ioex.Message, StatusCode = 400 });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var reservation = await _service.GetByIdAsync(id);
            if (reservation == null) return NotFound(new ApiResponse<object> { Success = false, Message = "No encontrado", StatusCode = 404 });

            var dto = _mapper.Map<ReservationDto>(reservation);
            return Ok(new ApiResponse<ReservationDto> { Success = true, Data = dto, StatusCode = 200 });
        }
    }
}
