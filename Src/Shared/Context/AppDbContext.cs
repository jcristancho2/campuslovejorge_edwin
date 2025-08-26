<<<<<<< HEAD
using campuslovejorge_edwin.Src.Modules.Administradores.Domain.Entities;
using campuslovejorge_edwin.Src.Modules.EstadisticasSistema.Domain.Entities;
using campuslovejorge_edwin.Src.Modules.User.Domain.Entities;
using campuslovejorge_edwin.Src.Modules.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Profile = campuslovejorge_edwin.Src.Modules.Users.Domain.Entities.Profile;
=======
using System.Linq;
using Microsoft.EntityFrameworkCore;
using campuslovejorge_edwin.Src.Modules.User.Domain.Entities;
>>>>>>> feature/module_user

namespace campuslovejorge_edwin.Src.Shared.Context
{

    public class AppDbContext : DbContext
    {
<<<<<<< HEAD
        
    }
    public DbSet<Administrado> Administrador { get; set; }
    public DbSet<Modules.EstadisticasSistema.Domain.Entities.Profile> Profile { get; set; }
    public DbSet<UsersLikes> UsersLikes { get; set; }
    public DbSet<Usuario> Users { get; set; }
    
    public DbSet<Gender> Genders { get; set; }
    public DbSet<Profession> Professions { get; set; }
    public DbSet<Interest> Interest { get; set; }
    public DbSet<InterestProfile> InterestProfiles { get; set; }
    public DbSet<Profile> Profiles { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
            modelBuilder.Entity<Usuario>(entity =>
    {
        entity.ToTable("users");

        entity.HasKey(u => u.Id);

        entity.Property(u => u.Id).HasColumnName("id");
        entity.Property(u => u.Username).HasColumnName("username");
        entity.Property(u => u.Password).HasColumnName("password");
        entity.Property(u => u.BirthDate).HasColumnName("birthdate");

        entity.Property(u => u.Profile_Id).HasColumnName("profile_id");

        entity.HasOne(u => u.Profile)
              .WithMany()
              .HasForeignKey(u => u.Profile_Id);
    });

    modelBuilder.Entity<Profile>(entity =>
    {
        entity.ToTable("profile");

        entity.HasKey(p => p.Id);

        entity.Property(p => p.Id).HasColumnName("id");
        entity.Property(p => p.Name).HasColumnName("name");
        entity.Property(p => p.Lastname).HasColumnName("lastname");
        entity.Property(p => p.Identification).HasColumnName("identification");
        entity.Property(p => p.Gender_Id).HasColumnName("gender_id");
        entity.Property(p => p.Slogan).HasColumnName("slogan");
        entity.Property(p => p.Status_Id).HasColumnName("status_id");
        entity.Property(p => p.CreateDate).HasColumnName("createDate");
        entity.Property(p => p.Profession_Id).HasColumnName("profession_id");
        entity.Property(p => p.Total_Likes).HasColumnName("total_likes");
    });
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
=======
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
>>>>>>> feature/module_user
    }
}
