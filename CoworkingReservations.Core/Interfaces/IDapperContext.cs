using CoworkingReservations.Core.Enum;
using System.Data;
using System.Threading.Tasks;

namespace CoworkingReservations.Core.Interfaces
{
    public interface IDapperContext
    {
        DatabaseProvider Provider { get; }
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null, CommandType commandType = CommandType.Text);
        Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, CommandType commandType = CommandType.Text);
        Task<int> ExecuteAsync(string sql, object? param = null, CommandType commandType = CommandType.Text);
        Task<T> ExecuteScalarAsync<T>(string sql, object? param = null, CommandType commandType = CommandType.Text);

        // Estos son para gestionar la transacción del UnitOfWork
        void SetAmbientConnection(IDbConnection conn, IDbTransaction? tx);
        void ClearAmbientConnection();
    }
}