using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using campuslovejorge_edwin.Src.Modules.Administradores.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace campuslovejorge_edwin.Src.Shared.Configuration
{
    public class AdministradorConfiguration 
    {
        public void Configure(EntityTypeBuilder<Administrado> builder)
        {
            builder.ToTable("administrador");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(a => a.Lastname)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(a => a.Identification)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(a => a.Username)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(a => a.Password)
                   .IsRequired()
                   .HasMaxLength(100);
        }
    }
}