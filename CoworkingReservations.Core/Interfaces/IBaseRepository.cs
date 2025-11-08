using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoworkingReservations.Core.Entities;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<IEnumerable<T>> GetAll();
    Task<T?> GetById(int id); // Usamos Task<T?> por si no se encuentra
    Task Add(T entity);
    Task Update(T entity);  // <--- DEBE SER ASÍNCRONO (Task)
    Task Delete(int id);
}
