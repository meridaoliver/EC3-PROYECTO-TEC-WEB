namespace CoworkingReservations.Core.DTOs
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? UserFullName { get; set; }
        public int SpaceId { get; set; }
        public string? SpaceName { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
