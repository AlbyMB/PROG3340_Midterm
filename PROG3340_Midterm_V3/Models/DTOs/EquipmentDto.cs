using System.ComponentModel.DataAnnotations;

namespace PROG3340_MidtermProject_V3.Models.DTOs
{
    public class EquipmentCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public decimal PriceRate { get; set; }
    }

    public class EquipmentResponseDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public decimal PriceRate { get; set; }

        public bool IsAvailable { get; set; }
    }
}
