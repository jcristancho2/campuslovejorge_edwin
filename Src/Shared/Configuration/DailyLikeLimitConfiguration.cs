using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using campuslovejorge_edwin.Src.Modules.Interaction.Domain.Entities;

namespace campuslovejorge_edwin.Src.Shared.Configuration
{
    public class DailyLikeLimitConfiguration : IEntityTypeConfiguration<DailyLikeLimit>
    {
        public void Configure(EntityTypeBuilder<DailyLikeLimit> entity)
        {
            entity.ToTable("daily_likes");
            entity.HasKey(u => u.DailyLikeLimitId);
            entity.Property(u => u.DailyLikeLimitId).HasColumnName("daily_like_limit_id");

            entity.Property(u => u.UserId)
                    .IsRequired()
                    .HasColumnName("user_id");

            entity.Property(u => u.LikesUsed)
                    .IsRequired()
                    .HasColumnName("likes_used")
                    .HasDefaultValue(0);

            entity.Property(u => u.MaxLikesPerDay)
                    .IsRequired()
                    .HasColumnName("max_likes_per_day")
                    .HasDefaultValue(10);

            entity.Property(u => u.Date)
                    .IsRequired()
                    .HasColumnName("date")
                    .HasDefaultValueSql("CURRENT_DATE");

            entity.Property(u => u.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(u => u.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");

            // Relación
            entity.HasOne(u => u.User)
                    .WithMany()
                    .HasForeignKey(u => u.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

            // Índice único para evitar múltiples registros por usuario por día
            entity.HasIndex(u => new { u.UserId, u.Date }).IsUnique();
        }
    }
}
