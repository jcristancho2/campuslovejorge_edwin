using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using campuslovejorge_edwin.Src.Modules.Interaction.Domain.Entities;

namespace campuslovejorge_edwin.Src.Shared.Configuration
{
    public class UserLikeConfiguration : IEntityTypeConfiguration<UserLike>
    {
        public void Configure(EntityTypeBuilder<UserLike> entity)
        {
            entity.ToTable("user_like");
            entity.HasKey(u => u.LikeId);
            entity.Property(u => u.LikeId).HasColumnName("like_id");

            entity.Property(u => u.LikerId)
                    .IsRequired()
                    .HasColumnName("liker_id");

            entity.Property(u => u.LikedId)
                    .IsRequired()
                    .HasColumnName("liked_id");

            entity.Property(u => u.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Relaciones
            entity.HasOne(u => u.Liker)
                    .WithMany()
                    .HasForeignKey(u => u.LikerId)
                    .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(u => u.Liked)
                    .WithMany()
                    .HasForeignKey(u => u.LikedId)
                    .OnDelete(DeleteBehavior.Restrict);

            // Índice para evitar likes duplicados
            entity.HasIndex(u => new { u.LikerId, u.LikedId }).IsUnique();
        }
    }
}
