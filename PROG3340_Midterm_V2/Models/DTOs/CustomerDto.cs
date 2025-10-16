using System.ComponentModel.DataAnnotations;

namespace PROG3340_MidtermProject_V2.Models.DTOs
{
    public class CustomerCreateDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
    public class CustomerResponseDto
    {
        public string Username { get; set; }

        public string Role { get; set; }

        public string Token { get; set; }
    }
}
