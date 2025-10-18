using System.ComponentModel.DataAnnotations;

namespace PROG3340_MidtermProject_V3.Models.Domain
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "User";


        public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    }
}
