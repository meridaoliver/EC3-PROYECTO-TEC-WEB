using System;
using System.Collections.Generic;

namespace CoworkingReservations.Core.Entities
{ 

public partial class Space : BaseEntity
{
   // public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int Capacity { get; set; }

    public bool IsAvailable { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
}
