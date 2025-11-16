using System.ComponentModel.DataAnnotations;

namespace FreightIT.InvoiceService
{
    public class Invoice
    {
        public int Id { get; set; }
        
        [Required]
        public string? InvoiceNumber { get; set; }
        
        [Required]
        public decimal Amount { get; set; }
        
        [Required]
        public string? ClientName { get; set; }
        
        public DateTime IssueDate { get; set; }
        
        public DateTime DueDate { get; set; }
        
        [Required]
        public string? Status { get; set; } // e.g., 'Pending', 'Paid'
    }
}

