using CoworkingReservations.Core.Entities;
using CoworkingReservations.Core.Interfaces;
using CoworkingReservations.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CoworkingReservations.Infrastructure.Persistence
{
    public class SpaceRepository : ISpaceRepository
    {
        private readonly TaskManagerContext _ctx;
        public SpaceRepository(TaskManagerContext ctx) => _ctx = ctx;
        public async Task<Space?> GetByIdAsync(int id) => await _ctx.Spaces.FindAsync(id);
    }
}
