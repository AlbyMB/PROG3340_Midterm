using System.ComponentModel.DataAnnotations;

namespace PROG3340_MidtermProject_V2.Models.DTOs
{
    public class RentalCreateDto
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int EquipmentId { get; set; }

        public string? ExtentionReason { get; set; }

        public string? Notes { get; set; }

        public string? Condition { get; set; }

        [Required]
        public DateTime RentalDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public DateTime? ReturnDate { get; set; }
    }

    public class RentalResponseDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int EquipmentId { get; set; }
        public string? ExtentionReason { get; set; }
        public string? Notes { get; set; }
        public string? Condition { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public decimal TotalCost { get; set; }
        public bool IsOverdue { get; set; }
    }
}
