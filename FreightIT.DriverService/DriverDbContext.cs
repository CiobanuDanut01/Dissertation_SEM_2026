using Microsoft.EntityFrameworkCore;

namespace FreightIT.DriverService
{
    public class DriverDbContext : DbContext
    {
        public DriverDbContext(DbContextOptions<DriverDbContext> options) : base(options)
        {
        }

        public DbSet<Driver> Drivers { get; set; } = null!;

        // Configure model
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Driver>(entity =>
            {
                // Map DateOfBirth to a date-only column type in the database
                entity.Property(d => d.DateOfBirth).HasColumnType("date");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
