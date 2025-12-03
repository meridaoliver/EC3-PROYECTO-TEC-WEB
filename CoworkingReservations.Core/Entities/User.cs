using System;
using System.Collections.Generic;
using CoworkingReservations.Core.Enum;
namespace CoworkingReservations.Core.Entities 
{

public partial class User : BaseEntity
{
    //public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

        // --- NUEVOS CAMPOS PARA EC4 ---
        public string Password { get; set; } = null!; // Aquí guardaremos el HASH
        public RoleType Role { get; set; }
        // ------------------------------
        public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}

}