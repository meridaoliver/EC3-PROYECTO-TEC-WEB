using CoworkingReservations.Core.Entities;
using CoworkingReservations.Core.Interfaces;
using CoworkingReservations.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CoworkingReservations.Infrastructure.Persistence
{
    public class UserRepository
    {
        private readonly TaskManagerContext _ctx;
        public UserRepository(TaskManagerContext ctx) => _ctx = ctx;
        public async Task<User?> GetByIdAsync(int id) => await _ctx.Users.FindAsync(id);
    }
}
