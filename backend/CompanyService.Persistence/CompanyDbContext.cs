using CompanyService.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyService.Persistence
{
    public sealed class CompanyDbContext : DbContext
    {
        public DbSet<CompanyEntity> Companies { get; set; }

        public CompanyDbContext(DbContextOptions<CompanyDbContext> options)
        : base(options) 
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompanyEntity>()
                .HasIndex(c => c.Isin)
                .IsUnique();

            modelBuilder.Entity<CompanyEntity>()
                .HasIndex(c => c.Ticker);

            modelBuilder.Entity<CompanyEntity>()
                .HasIndex(c => c.Cursor);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CompanyEntity>().HasData(SeedData.GetCompanies());
        }
    }
}
