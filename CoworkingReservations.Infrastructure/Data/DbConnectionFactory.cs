using Microsoft.Data.SqlClient; // Para SQL Server
using Microsoft.Extensions.Configuration;
using CoworkingReservations.Core.Enum;
using CoworkingReservations.Core.Interfaces;
using System.Data;

namespace CoworkingReservations.Infrastructure.Data
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _sqlConn;
        public DbConnectionFactory(IConfiguration config)
        {
            //[cite_start]// OJO: Apunta al nombre de tu ConnectionString en appsettings.json [cite: 2038]
            _sqlConn = config.GetConnectionString("DefaultConnection") ?? string.Empty;
            Provider = DatabaseProvider.SqlServer; // Tu proyecto solo usa SQL Server [cite: 2038]
        }
        public DatabaseProvider Provider { get; }
        public IDbConnection CreateConnection() => new SqlConnection(_sqlConn);
    }
}