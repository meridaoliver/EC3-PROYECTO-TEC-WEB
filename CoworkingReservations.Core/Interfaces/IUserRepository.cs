using CoworkingReservations.Core.Entities;

namespace CoworkingReservations.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
    }
}
