using Microsoft.EntityFrameworkCore;
using MyCleanArchitectureApp.Core.Entities;

namespace MyCleanArchitectureApp.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>()
                .HasMany(c => c.States)
                .WithOne(s => s.Country)
                .HasForeignKey(s => s.CountryId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
