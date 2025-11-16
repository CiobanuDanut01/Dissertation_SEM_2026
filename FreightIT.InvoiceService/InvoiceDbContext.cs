using Microsoft.EntityFrameworkCore;

namespace FreightIT.InvoiceService
{
    public class InvoiceDbContext : DbContext
    {
        public InvoiceDbContext(DbContextOptions<InvoiceDbContext> options)
            : base(options) 
        { 
        }

        public DbSet<Invoice> Invoices { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Invoice entity to use timestamp without time zone
            // This prevents the DateTime Kind=Local error with PostgreSQL
            modelBuilder.Entity<Invoice>()
                .Property(i => i.IssueDate)
                .HasColumnType("timestamp");

            modelBuilder.Entity<Invoice>()
                .Property(i => i.DueDate)
                .HasColumnType("timestamp");
        }
    }
}

