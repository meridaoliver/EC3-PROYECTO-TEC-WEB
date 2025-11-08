using CoworkingReservations.Core.Entities;
using CoworkingReservations.Core.Interfaces;
using CoworkingReservations.Infrastructure.Data; // Necesario para IDapperContext
using CoworkingReservations.Infrastructure.Persistence; // Necesario para TaskManagerContext
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Threading.Tasks;

namespace CoworkingReservations.Infrastructure.Repositories
{
    // Implementa IUnitOfWork (que incluye IDisposable)
    public class UnitOfWork : IUnitOfWork
    {
        // 1. CAMPOS: Ahora son readonly y se inicializan en el constructor
        private readonly TaskManagerContext _context;
        private readonly IDapperContext _dapper;
        private IDbContextTransaction? _efTransaction; // Para sincronizar transacciones EF Core y Dapper

        // 2. CAMPOS PARA REPOSITORIOS (Lazy Loading)
        private IReservationRepository? _reservationRepository;
        private IBaseRepository<User>? _userRepository;
        private IBaseRepository<Space>? _spaceRepository;

        // 3. CONSTRUCTOR: Inicializa los contextos requeridos
        public UnitOfWork(TaskManagerContext context, IDapperContext dapper)
        {
            // OJO: Es fundamental inyectar ambos contextos
            _context = context;
            _dapper = dapper;
        }

        // 4. PROPIEDADES DE REPOSITORIOS (Lazy Loading - Social Media Pattern)
        public IReservationRepository ReservationRepository =>
            // Usamos ReservationRepository (que inyecta ambos contextos)
            _reservationRepository ??= new ReservationRepository(_context, _dapper);

        public IBaseRepository<User> UserRepository =>
            // Usamos BaseRepository<User> (repositorio genérico de EF Core)
            _userRepository ??= new BaseRepository<User>(_context);

        public IBaseRepository<Space> SpaceRepository =>
            // Usamos BaseRepository<Space> (repositorio genérico de EF Core)
            _spaceRepository ??= new BaseRepository<Space>(_context);


        // 5. MÉTODOS DE PERSISTENCIA (EF Core)
        public void SaveChanges() => _context.SaveChanges();
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        // 6. MÉTODOS DE TRANSACCIÓN (Sincronización EF Core y Dapper)
        public async Task BeginTransaccionAsync()
        {
            if (_efTransaction == null)
            {
                // Iniciar la transacción de EF Core
                _efTransaction = await _context.Database.BeginTransactionAsync();

                // Registrar la conexión/transacción en Dapper para que use la misma
                var conn = _context.Database.GetDbConnection();
                var tx = _efTransaction.GetDbTransaction();
                _dapper.SetAmbientConnection(conn, tx);
            }
        }

        public async Task CommitAsync()
        {
            try
            {
                await SaveChangesAsync(); // Guarda los cambios de EF Core
                if (_efTransaction != null)
                {
                    await _efTransaction.CommitAsync();
                    _efTransaction.Dispose();
                    _efTransaction = null;
                }
            }
            finally
            {
                _dapper.ClearAmbientConnection(); // Siempre limpiar la conexión ambiental de Dapper
            }
        }

        public async Task RollbackAsync()
        {
            if (_efTransaction != null)
            {
                await _efTransaction.RollbackAsync();
                _efTransaction.Dispose();
                _efTransaction = null;
            }
            _dapper.ClearAmbientConnection(); // Siempre limpiar la conexión ambiental de Dapper
        }

        // 7. MÉTODOS DE CONEXIÓN (Para Dapper, si se necesita conexión externa)
        public IDbConnection? GetDbConnection() => _context.Database.GetDbConnection();
        public IDbTransaction? GetDbTransaction() => _efTransaction?.GetDbTransaction();

        // 8. IMPLEMENTACIÓN DE IDisposable
        public void Dispose()
        {
            // Disponer la transacción de EF Core si existe
            if (_efTransaction != null)
            {
                _efTransaction.Dispose();
            }
            // Disponer el contexto de EF Core
            _context.Dispose();
            // Limpiar Dapper
            _dapper.ClearAmbientConnection();
            GC.SuppressFinalize(this); // Opcional, pero buena práctica
        }
    }
}