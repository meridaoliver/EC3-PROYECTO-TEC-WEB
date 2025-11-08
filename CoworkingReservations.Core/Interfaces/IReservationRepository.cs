using CoworkingReservations.Core.Entities;

namespace CoworkingReservations.Core.Interfaces
{

    public interface IReservationRepository : IBaseRepository<Reservation>
    {
        Task Add(object reservation);
        Task<bool> HasConflictAsync(int spaceId, DateTime start, DateTime end);
    }
}
