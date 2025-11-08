using System;
using System.Collections.Generic;

namespace CoworkingReservations.Core.Entities 
{

public partial class User : BaseEntity
{
    //public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}

}