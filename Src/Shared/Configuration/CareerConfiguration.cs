using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using campuslovejorge_edwin.Src.Modules.User.Domain.Entities;

namespace campuslovejorge_edwin.Src.Shared.Configuration
{
    public class CareerConfiguration : IEntityTypeConfiguration<Career>
    {
        public void Configure(EntityTypeBuilder<Career> entity)
        {
            entity.ToTable("career");
            entity.HasKey(u => u.CareerId);
            entity.Property(u => u.CareerId).HasColumnName("career_id");

            entity.Property(u => u.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");

            entity.Property(u => u.Category)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("category");

            entity.Property(u => u.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Índice para búsquedas por categoría
            entity.HasIndex(u => u.Category);
        }
    }
}
