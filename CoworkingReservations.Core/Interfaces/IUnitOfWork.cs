// Archivo: CoworkingReservations.Core/Interfaces/IUnitOfWork.cs
using System;
using System.Data;
using System.Threading.Tasks;
using CoworkingReservations.Core.Entities;
using CoworkingReservations.Core.Interfaces;


public interface IUnitOfWork : IDisposable
{
    IReservationRepository ReservationRepository { get; }
    IBaseRepository<User> UserRepository { get; }  
    IBaseRepository<Space> SpaceRepository { get; } 

    // Métodos de Persistencia (EF Core)
    void SaveChanges();
    Task SaveChangesAsync();

    // Métodos de Transacción (para Dapper y EF Core)
    Task BeginTransaccionAsync();
    Task CommitAsync();
    Task RollbackAsync();

    // Propiedades de Conexión (Necesarias para Dapper)
    IDbConnection? GetDbConnection();
    IDbTransaction? GetDbTransaction();
}