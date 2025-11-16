using System.ComponentModel.DataAnnotations;

namespace FreightIT.TruckService
{
    public class Truck
    {
        public int Id { get; set; }
        [Required]
        public string? LicensePlate { get; set; }
        [Required]
        public string? Make { get; set; }
        [Required]
        public string? Model { get; set; }
        [Range(1950, 2100)] // A new "rule": Year must be between 1950-2100
        public int Year { get; set; }
        [Range(0, 5000000)]
        public int Mileage { get; set; }
    }
}