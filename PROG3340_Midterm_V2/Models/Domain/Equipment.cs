using System.ComponentModel.DataAnnotations;

namespace PROG3340_MidtermProject_V2.Models.Domain
{
    public class Equipment
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        // PriceRate is the rental price per day
        [Required]
        public decimal PriceRate { get; set; }

        [Required]
        public string Category { get; set; }

        // All Rentals are returned => Equipment is available
        public bool IsAvailable { get => Rentals.All(r => r.ReturnDate != null); }


        public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    }
}
