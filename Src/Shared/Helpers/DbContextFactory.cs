using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using campuslovejorge_edwin.Src.Shared.Context;

namespace campuslovejorge_edwin.Src.Shared.Helpers
{
    public class DbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseMySql("Server=localhost;Database=test;User=root;Password=;", 
                new MySqlServerVersion(new Version(8, 0, 21)));

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
