using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using campuslovejorge_edwin.Src.Modules.Interaction.Domain.Entities;

namespace campuslovejorge_edwin.Src.Shared.Configuration
{
    public class MatchConfiguration : IEntityTypeConfiguration<Match>
    {
        public void Configure(EntityTypeBuilder<Match> entity)
        {
            entity.ToTable("match_table");
            entity.HasKey(u => u.MatchId);
            entity.Property(u => u.MatchId).HasColumnName("match_id");

            entity.Property(u => u.User1Id)
                    .IsRequired()
                    .HasColumnName("user1_id");

            entity.Property(u => u.User2Id)
                    .IsRequired()
                    .HasColumnName("user2_id");

            entity.Property(u => u.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Relaciones
            entity.HasOne(u => u.User1)
                    .WithMany()
                    .HasForeignKey(u => u.User1Id)
                    .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(u => u.User2)
                    .WithMany()
                    .HasForeignKey(u => u.User2Id)
                    .OnDelete(DeleteBehavior.Restrict);

            // Índice para evitar matches duplicados
            entity.HasIndex(u => new { u.User1Id, u.User2Id }).IsUnique();
        }
    }
}
