using System.ComponentModel.DataAnnotations;

namespace FreightIT.DriverService
{
    public class Driver
    {
        // The unique ID number for the driver.
        // 'get; set;' is C# shorthand for "this is a piece of data."
        public int Id { get; set; }

        // The '?' after 'string' means this value is "nullable,"
        // which means it's allowed to be empty (null).
        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required]
        public string? LicenseNumber { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}
