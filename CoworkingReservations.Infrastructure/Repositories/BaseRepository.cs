using CoworkingReservations.Core.Entities;
using CoworkingReservations.Core.Interfaces;
using CoworkingReservations.Infrastructure.Persistence; // OJO: Tu DbContext está en "Infrastructure/Repositories" según tu archivo, ajusta el using si es necesario.
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoworkingReservations.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly TaskManagerContext _context;
        protected readonly DbSet<T> _entities;

        public BaseRepository(TaskManagerContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        // ---------- ¡AQUÍ ESTÁ LA CORRECCIÓN 1! ----------
        // Añadimos 'virtual' para que las clases hijas (como ReservationRepository)
        // puedan sobrescribir este método (por ejemplo, para usar Dapper).
        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await _entities.ToListAsync();
        }

        // ---------- ¡AQUÍ ESTÁ LA CORRECCIÓN 2! ----------
        // Añadimos 'virtual' también aquí.
        public virtual async Task<T?> GetById(int id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task Add(T entity)
        {
            await _entities.AddAsync(entity);
        }

        // (Este es el método que corregimos antes, déjalo así)
        public async Task Update(T entity)
        {
            _entities.Update(entity);
            await Task.CompletedTask;
        }

        public async Task Delete(int id)
        {
            T? entity = await GetById(id);
            if (entity != null)
            {
                _entities.Remove(entity);
            }
        }
    }
}