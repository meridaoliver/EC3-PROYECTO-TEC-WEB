using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoworkingReservations.Infrastructure.Queries
{
    // Esta clase almacena tus SQL de Dapper como constantes
    public static class ReservationQueries
    {
        // Query para GET por ID (incluye joins)
        public const string GetReservationById = @"
            SELECT r.*, 
                   u.FullName AS UserFullName, 
                   s.Name AS SpaceName
            FROM Reservations r
            LEFT JOIN Users u ON r.UserId = u.Id
            LEFT JOIN Spaces s ON r.SpaceId = s.Id
            WHERE r.Id = @Id;";

        // Query para GET ALL (base para filtros)
        public const string GetAllReservations = @"
            SELECT r.*, 
                   u.FullName AS UserFullName, 
                   s.Name AS SpaceName
            FROM Reservations r
            LEFT JOIN Users u ON r.UserId = u.Id
            LEFT JOIN Spaces s ON r.SpaceId = s.Id
            /**filters**/
            /**orderby**/
            /**pagination**/";
    }
}
