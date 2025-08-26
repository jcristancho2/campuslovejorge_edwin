using campuslovejorge_edwin.Src.Modules.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace campuslovejorge_edwin.Src.Shared.Configuration
{
    public class UsersConfiguration : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.ToTable("profile");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(100);
            

            builder.HasOne(p => p.Profession)            // navegación
                   .WithMany(pr => pr.Profiles)          // colección en Profession
                   .HasForeignKey(p => p.Profession_Id);  // FK
            builder.HasOne(p => p.Gender)
                   .WithMany()
                   .HasForeignKey(p => p.Gender_Id);
        }
    }

    public class InterestConfiguration : IEntityTypeConfiguration<Interest>
    {
        public void Configure(EntityTypeBuilder<Interest> builder)
        {
            builder.ToTable("interest");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                   .HasColumnName("id");

            builder.Property(i => i.Description)
                   .HasColumnName("description");
        }
    }

    public class ProfessionConfiguration : IEntityTypeConfiguration<Profession>
    {
        public void Configure(EntityTypeBuilder<Profession> builder)
        {
            builder.ToTable("profession");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Description)
                   .IsRequired()
                   .HasMaxLength(100);
        }
        public class InterestProfileConfiguration : IEntityTypeConfiguration<InterestProfile>
        {
            public void Configure(EntityTypeBuilder<InterestProfile> builder)
            {
            builder.ToTable("interest_profile");

            builder.HasKey(ip => new { ip.Profile_Id, ip.Interest_Id });

            builder.Property(ip => ip.Profile_Id)
                   .HasColumnName("profile_id");

            builder.Property(ip => ip.Interest_Id)
                   .HasColumnName("interest_id");

            builder.HasOne(ip => ip.Profile)
                   .WithMany(p => p.Interest)
                   .HasForeignKey(ip => ip.Profile_Id);

            builder.HasOne(ip => ip.Interest)
                   .WithMany(i => i.Profiles)
                   .HasForeignKey(ip => ip.Interest_Id);
            }
        }
    }
}
