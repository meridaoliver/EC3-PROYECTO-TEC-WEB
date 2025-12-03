using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CoworkingReservations.Core.Entities; // Asegúrate de tener este using

namespace CoworkingReservations.Infrastructure.Data.Configurations
{
    // 1. La clase debe heredar de IEntityTypeConfiguration<User>
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        // 2. Todo el código de configuración va DENTRO de este método
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Configuración de la llave primaria (ya la tenías)
            builder.HasKey(e => e.Id).HasName("PK_Usuario");
            builder.ToTable("Users"); // O el nombre que tenga tu tabla en la BD

            // Configuraciones existentes (ya las tenías)
            builder.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.FullName)
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);

            // --- ¡AQUÍ ES DONDE AGREGAS LO NUEVO! ---

            builder.Property(e => e.Password)
                   .HasMaxLength(200) // Espacio suficiente para el hash
                   .IsUnicode(false)
                   .IsRequired(); // Generalmente el password es obligatorio

            builder.Property(e => e.Role)
                   .HasConversion<string>() // Guardamos "Administrator" o "User" como texto
                   .HasMaxLength(15)
                   .IsRequired();

            // ----------------------------------------
        }
    }
}

