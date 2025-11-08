namespace CoworkingReservations.Core.DTOs
{
    public class SpaceDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int Capacity { get; set; }
        public bool IsAvailable { get; set; }
    }
}
