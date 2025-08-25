using campuslovejorge_edwin.Src.Modules.Administradores.Domain.Entities;
using campuslovejorge_edwin.Src.Modules.EstadisticasSistema.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace campuslovejorge_edwin.Src.Shared.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Administrado> Administrador { get; set; }
    public DbSet<Profile> Profile { get; set; }
    public DbSet<UsersLikes> UsersLikes { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
