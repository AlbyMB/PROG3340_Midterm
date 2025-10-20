namespace PROG3340_Midterm.Web.Models
{
	public record TokenResponse(string Token, string Role, string? Username);

	public record EquipmentDto
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public int Category { get; set; }
		public int Condition { get; set; }
		public decimal RentalPrice { get; set; }
		public string? Description { get; set; }
		public bool IsAvailable { get; set; }
	}

	public record CustomerDto
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string? Email { get; set; }
		public string UserName { get; set; } = string.Empty;
		public string? Password { get; set; }
		public string Role { get; set; } = string.Empty;
	}

	public record RentalDto
	{
		public int Id { get; set; }
		public int EquipmentId { get; set; }
		public int CustomerId { get; set; }
		public DateTime IssuedAt { get; set; }
		public DateTime? ReturnedAt { get; set; }
		public DateTime DueDate { get; set; }
		public string? Status { get; set; }
		public string? ReturnCondition { get; set; }
		public string? ExtensionReason { get; set; }
		public string? Notes { get; set; }
	}

	public record EquipmentDetailsDto
	{
		public EquipmentDto Equipment { get; set; } = new();
		public List<RentalDto> RentalHistory { get; set; } = new();
	}
}
