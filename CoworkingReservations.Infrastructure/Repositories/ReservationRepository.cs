using CoworkingReservations.Core.Entities;
using CoworkingReservations.Core.Interfaces;
using CoworkingReservations.Infrastructure.Data; // Tu DapperContext
using CoworkingReservations.Infrastructure.Persistence; // Tu DbContext (TaskManagerContext)
using CoworkingReservations.Infrastructure.Queries;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoworkingReservations.Infrastructure.Repositories
{
    public class ReservationRepository : BaseRepository<Reservation>, IReservationRepository
    {
        private readonly TaskManagerContext _ctx;
        private readonly IDapperContext _dapper;

        public ReservationRepository(TaskManagerContext ctx, IDapperContext dapper) : base(ctx)
        {
            _ctx = ctx;
            _dapper = dapper;
        }

        // ---------- ¡AQUÍ ESTÁ LA CORRECCIÓN 1! ----------
        // Usamos 'override' porque el método base (en BaseRepository)
        // ahora está marcado como 'virtual'.
        public override async Task<Reservation?> GetById(int id)
        {
            // Ahora 'ReservationQueries' SÍ existe
            return await _dapper.QueryFirstOrDefaultAsync<Reservation>(
                ReservationQueries.GetReservationById, new { Id = id });
        }

        // ---------- ¡AQUÍ ESTÁ LA CORRECCIÓN 2! ----------
        // Usamos 'override' también aquí.
        public override async Task<IEnumerable<Reservation>> GetAll()
        {
            string sql = ReservationQueries.GetAllReservations
                .Replace("/**filters**/", "")
                .Replace("/**orderby**/", "ORDER BY r.StartDateTime DESC")
                .Replace("/**pagination**/", "");

            return await _dapper.QueryAsync<Reservation>(sql);
        }

        // Tu método personalizado (que SÍ estaba en tu archivo)
        // Sigue usando EF Core porque es una validación simple.
        public async Task<bool> HasConflictAsync(int spaceId, DateTime start, DateTime end) =>
            await _ctx.Reservations.AnyAsync(r =>
                r.SpaceId == spaceId &&
                r.Status == ReservationStatus.Booked &&
                r.StartDateTime < end &&
                r.EndDateTime > start);

        public Task Add(object reservation)
        {
            throw new NotImplementedException();
        }
    }
}