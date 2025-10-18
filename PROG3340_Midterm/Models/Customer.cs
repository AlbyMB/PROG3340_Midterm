using System.ComponentModel.DataAnnotations;

namespace PROG3340_Midterm.Models
{
	public class Customer
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		public string? Email { get; set; }

		[Required]
		public string UserName { get; set; }
		[Required]
		public string Password { get; set; }    
		[Required]
		public string Role { get; set; }  
		public ICollection<Rental> Rentals { get; set; }
	}
}
