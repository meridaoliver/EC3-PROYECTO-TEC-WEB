using CoworkingReservations.Core.DTOs;
using CoworkingReservations.Core.Entities;

namespace CoworkingReservations.Core.Interfaces
{
    public interface IReservationService
    {
        Task<Reservation?> CreateReservationAsync(ReservationCreateDto dto);
        Task<Reservation?> GetByIdAsync(int id);
    }
}
