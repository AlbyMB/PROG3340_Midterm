using System.ComponentModel.DataAnnotations;

namespace PROG3340_Midterm.Models
{
	public class ProfileUpdateRequest
	{
		[Required]
		public string Name { get; set; } = string.Empty;
		public string? Email { get; set; }
		// Optional; if null or empty, keep existing password
		public string? Password { get; set; }
	}
}
