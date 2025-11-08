using CoworkingReservations.Core.Entities;

namespace CoworkingReservations.Core.Interfaces
{
    public interface ISpaceRepository
    {
        Task<Space?> GetByIdAsync(int id);
    }
}
