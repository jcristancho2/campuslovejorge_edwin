using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using campuslovejorge_edwin.Src.Modules.User.Domain.Entities;

namespace campuslovejorge_edwin.Src.Shared.Configuration
{
    public class UserInterestConfiguration : IEntityTypeConfiguration<UserInterest>
    {
        public void Configure(EntityTypeBuilder<UserInterest> entity)
        {
            entity.ToTable("user_interest");
            
            // Clave primaria compuesta
            entity.HasKey(u => new { u.UserId, u.InterestId });

            entity.Property(u => u.UserId)
                    .HasColumnName("user_id");

            entity.Property(u => u.InterestId)
                    .HasColumnName("interest_id");

            entity.Property(u => u.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Relaciones
            entity.HasOne(u => u.User)
                    .WithMany()
                    .HasForeignKey(u => u.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(u => u.Interest)
                    .WithMany()
                    .HasForeignKey(u => u.InterestId)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
