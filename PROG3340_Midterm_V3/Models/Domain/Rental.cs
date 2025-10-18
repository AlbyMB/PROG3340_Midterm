using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROG3340_MidtermProject_V3.Models.Domain
{
    public class Rental
    {
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int EquipmentId { get; set; }

        public string? ExtentionReason { get; set; } = null;

        public string? Notes { get; set; } = null;

        public string? Condition { get; set; } = null;

        [Required]
        public DateTime RentalDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public DateTime? ReturnDate { get; set; } = null;

        // Price of rental is calculated as the number of days rented * Equipment.PriceRate
        public decimal TotalCost { get => Equipment?.PriceRate * (DueDate - RentalDate).Days ?? 0; }

        public bool IsOverdue { get => DateTime.Now > DueDate && ReturnDate == null; }


        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        [ForeignKey("EquipmentId")]
        public virtual Equipment? Equipment { get; set; }
    }
}
