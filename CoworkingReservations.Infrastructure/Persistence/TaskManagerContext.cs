using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CoworkingReservations.Core.Entities;

namespace CoworkingReservations.Infrastructure.Persistence
{
    public partial class TaskManagerContext : DbContext
    {
        public TaskManagerContext(DbContextOptions<TaskManagerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Space> Spaces { get; set; } = null!;
        public virtual DbSet<Reservation> Reservations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ajustes mínimos (si scaffold ya creó mappings, mantenlos)
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<Reservation>().Property(r => r.Status).HasConversion<int>();
        }
    }
}
