using System.ComponentModel.DataAnnotations;

namespace PROG3340_MidtermProject_V3.Models.DTOs
{
    public class CustomerCreateDto
    {
        
        public string Username { get; set; }

        public string Password { get; set; }
    }
    public class CustomerResponseDto
    {
        public string Username { get; set; }

        public string Role { get; set; }

        public string Token { get; set; }
    }
}
