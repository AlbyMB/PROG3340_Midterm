namespace PROG3340_Midterm.Web.Models
{
	public record CustomerDetailsDto
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string? Email { get; set; }
		public string UserName { get; set; } = string.Empty;
		public string Role { get; set; } = string.Empty;
		public List<RentalDto> Rentals { get; set; } = new();
	}

	public record RentedEquipmentResponse
	{
		public int Count { get; set; }
		public List<EquipmentDto> Equipment { get; set; } = new();
	}
}