namespace CoworkingReservations.Core.DTOs
{
    public class ReservationCreateDto
    {
        public int UserId { get; set; }
        public int SpaceId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}
