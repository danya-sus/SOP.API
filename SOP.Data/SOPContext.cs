using Microsoft.EntityFrameworkCore;
using SOP.Models.Entities;

namespace SOP.Data
{
    public class SOPContext : DbContext
    {
        public SOPContext(DbContextOptions<SOPContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSnakeCaseNamingConvention();
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Owner> Owners { get; set; }
    }
}
