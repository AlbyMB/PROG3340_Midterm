namespace PROG3340_Midterm.Models
{
	public enum EquipmentCategory { HeavyMachinery, PowerTools, Vehicles, Safety, Surveying }
	public enum EquipmentCondition { New, Excellent, Good, Fair, Poor }

	public class Equipment
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public EquipmentCategory Category { get; set; }
		public EquipmentCondition Condition { get; set; }
		public decimal RentalPrice { get; set; }
		public string? Description { get; set; }
		public bool IsAvailable { get; set; } = true;
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public ICollection<Rental>? Rentals { get; set; }
	}
}
