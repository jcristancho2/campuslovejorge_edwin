using campuslovejorge_edwin.Src.Modules.Administradores.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace campuslovejorge_edwin.Src.Shared.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Administrado> Administrador { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
