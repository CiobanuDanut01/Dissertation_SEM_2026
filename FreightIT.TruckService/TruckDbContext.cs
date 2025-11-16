using Microsoft.EntityFrameworkCore;

namespace FreightIT.TruckService
{
    public class TruckDbContext : DbContext
    {
        public TruckDbContext(DbContextOptions<TruckDbContext> options)
            : base(options) { }

        public DbSet<Truck> Trucks { get; set; }
    }
}