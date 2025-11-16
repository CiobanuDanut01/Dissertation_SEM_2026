using Microsoft.EntityFrameworkCore;

namespace FreightIT.OrderService
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options)
            : base(options) 
        { 
        }

        public DbSet<TransportOrder> TransportOrders { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure TransportOrder entity to use timestamp without time zone
            // This prevents the DateTime Kind=Local error with PostgreSQL
            modelBuilder.Entity<TransportOrder>()
                .Property(o => o.PickupDate)
                .HasColumnType("timestamp");

            modelBuilder.Entity<TransportOrder>()
                .Property(o => o.DeliveryDate)
                .HasColumnType("timestamp");
        }
    }
}

