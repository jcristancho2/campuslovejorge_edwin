using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using campuslovejorge_edwin.Src.Modules.User.Domain.Entities;

namespace campuslovejorge_edwin.Src.Shared.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> entity)
        {
            entity.ToTable("user");
            entity.HasKey(u => u.UserId);
            entity.Property(u => u.UserId).HasColumnName("user_id");

            entity.Property(u => u.FullName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("fullname");

            entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("email");

            entity.HasIndex(u => u.Email).IsUnique();

            entity.Property(u => u.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("password_hash");

            entity.Property(u => u.Birthdate)
                    .IsRequired()
                    .HasColumnName("birthdate");

            entity.Property(u => u.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(u => u.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");

            entity.Property(u => u.GenderId)
                    .IsRequired()
                    .HasColumnName("gender_id");

            entity.Property(u => u.OrientationId)
                    .IsRequired()
                    .HasColumnName("orientation_id");

            // Relaciones (si agregas las tablas de catálogo)
            // entity.HasOne<Gender>().WithMany().HasForeignKey(u => u.GenderId);
            // entity.HasOne<Orientation>().WithMany().HasForeignKey(u => u.OrientationId);
        }
    }
}


