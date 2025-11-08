using CoworkingReservations.Core.Entities;

namespace CoworkingReservations.Core.Entities
{
    public enum ReservationStatus { Booked = 0, Cancelled = 1 };

    //      PASO 1: Asegúrate de que ": BaseEntity" esté aquí
    public partial class Reservation : BaseEntity
    {
        // PASO 2: Asegúrate de que esta línea esté borrada o comentada
        // public int Id { get; set; } 

        public int UserId { get; set; }

        public int SpaceId { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public ReservationStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual Space Space { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}