namespace PROG3340_Midterm.Models
{
	public class Rental
	{
		public int Id { get; set; }
		public int EquipmentId { get; set; }
		public Equipment? Equipment { get; set; }
		public int CustomerId { get; set; }
		public Customer? Customer { get; set; }
		public DateTime? IssuedAt { get; set; }
		public DateTime? ReturnedAt { get; set; }
		public DateTime DueDate { get; set; }       // for overdue logic
		public string? Status { get; set; }          // e.g., "Active", "Completed", "Cancelled"
		public string? ReturnCondition { get; set; }
		public string? ExtensionReason { get; set; }
		public string? Notes { get; set; }
	}
}
