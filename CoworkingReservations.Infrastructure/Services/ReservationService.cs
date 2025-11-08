using CoworkingReservations.Core.Interfaces;
using CoworkingReservations.Core.DTOs;
using CoworkingReservations.Core.Entities;
using AutoMapper;

namespace CoworkingReservations.Infrastructure.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IUnitOfWork _unitOfWork; // <-- Inyectamos UoW
        private readonly IMapper _mapper;
        private object reservation;

        public ReservationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Reservation?> CreateReservationAsync(ReservationCreateDto dto)
        {
            // ...
            // Usamos la UoW para acceder al repositorio (que obtiene AddAsync de BaseRepository)
            await _unitOfWork.ReservationRepository.Add(reservation); // <-- Arregla Error 7

            // ¡SaveChangesAsync AHORA SE LLAMA DESDE LA UOW!
            await _unitOfWork.SaveChangesAsync(); // <-- Arregla Error 8
                                                  // ...
            return (Reservation?)reservation;
        }

        public Task<Reservation?> GetByIdAsync(int id)
        {
            // Usamos la UoW
            return _unitOfWork.ReservationRepository.GetById(id);
        }
    }
}