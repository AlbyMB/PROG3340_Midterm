namespace PROG3340_Midterm.Web.Models
{
	public class RentalListItem
	{
		public int Id { get; set; }
		public int EquipmentId { get; set; }
		public string EquipmentName { get; set; } = string.Empty;
		public int CustomerId { get; set; }
		public string CustomerName { get; set; } = string.Empty;
		public DateTime IssuedAt { get; set; }
		public DateTime? ReturnedAt { get; set; }
		public DateTime DueDate { get; set; }
		public string? Status { get; set; }
	}
}
