using System.ComponentModel.DataAnnotations;

namespace FreightIT.OrderService
{
    public class TransportOrder
    {
        public int Id { get; set; }
        
        [Required]
        public string? OriginAddress { get; set; }
        
        [Required]
        public string? DestinationAddress { get; set; }
        
        public DateTime PickupDate { get; set; }
        
        public DateTime DeliveryDate { get; set; }
        
        [Required]
        public string? Status { get; set; } // e.g., 'Pending', 'EnRoute', 'Completed'
        
        [Required]
        public int DriverId { get; set; }
        
        [Required]
        public int TruckId { get; set; }
        
        [Required]
        public int InvoiceId { get; set; }
        
        // This is for the AI later - nullable string
        public string? RecommendedRoute { get; set; }
    }
}

