using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using campuslovejorge_edwin.Src.Modules.User.Domain.Entities;

namespace campuslovejorge_edwin.Src.Shared.Configuration
{
    public class InterestConfiguration : IEntityTypeConfiguration<Interest>
    {
        public void Configure(EntityTypeBuilder<Interest> entity)
        {
            entity.ToTable("interest");
            entity.HasKey(u => u.InterestId);
            entity.Property(u => u.InterestId).HasColumnName("interest_id");

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
