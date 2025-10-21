using Microsoft.EntityFrameworkCore;
using PROG3340_Midterm.Models;

namespace PROG3340_Midterm.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<Equipment> Equipments { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Rental> Rentals { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Rental>()
				.HasOne(r => r.Customer)
				.WithMany(c => c.Rentals)
				.HasForeignKey(r => r.CustomerId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Rental>()
				.HasOne(r => r.Equipment)
				.WithMany(e => e.Rentals)
				.HasForeignKey(r => r.EquipmentId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Customer>().HasData(
				new Customer { Id = 1, UserName = "admin", Password = "admin123", Role = "Admin", Email = "admin@site.com", Name = "Administrator" },
				new Customer { Id = 2, UserName = "alice", Password = "user123", Role = "User", Email = "alice@site.com", Name = "Alice Johnson" },
				new Customer { Id = 3, UserName = "bob", Password = "user123", Role = "User", Email = "bob@site.com", Name = "Bob Smith" },
				new Customer { Id = 4, UserName = "carol", Password = "user123", Role = "User", Email = "carol@site.com", Name = "Carol Lee" },
				new Customer { Id = 5, UserName = "dave", Password = "user123", Role = "User", Email = "dave@site.com", Name = "Dave Brown" }
			);

			var today = DateTime.Today;

			modelBuilder.Entity<Rental>().HasData(
				new Rental { Id = 1, CustomerId = 2, EquipmentId = 1, IssuedAt = today.AddDays(-14), DueDate = today.AddDays(-7), ReturnedAt = today.AddDays(-6), Status = "Completed", ReturnCondition = "Good" },
				new Rental { Id = 2, CustomerId = 3, EquipmentId = 2, IssuedAt = today.AddDays(-12), DueDate = today.AddDays(-6), ReturnedAt = today.AddDays(-5), Status = "Completed", ReturnCondition = "Good" },
				new Rental { Id = 6, CustomerId = 3, EquipmentId = 1, IssuedAt = today.AddDays(-30), DueDate = today.AddDays(-25), ReturnedAt = today.AddDays(-24), Status = "Completed", ReturnCondition = "Fair" },
				new Rental { Id = 7, CustomerId = 4, EquipmentId = 2, IssuedAt = today.AddDays(-22), DueDate = today.AddDays(-18), ReturnedAt = today.AddDays(-17), Status = "Completed", ReturnCondition = "Good" },
				new Rental { Id = 8, CustomerId = 5, EquipmentId = 3, IssuedAt = today.AddDays(-18), DueDate = today.AddDays(-15), ReturnedAt = today.AddDays(-14), Status = "Completed", ReturnCondition = "Good" },
				new Rental { Id = 9, CustomerId = 1, EquipmentId = 4, IssuedAt = today.AddDays(-40), DueDate = today.AddDays(-35), ReturnedAt = today.AddDays(-34), Status = "Completed", ReturnCondition = "Good" },
				new Rental { Id = 10, CustomerId = 1, EquipmentId = 2, IssuedAt = today.AddDays(-50), DueDate = today.AddDays(-45), ReturnedAt = today.AddDays(-44), Status = "Completed", ReturnCondition = "Good" },

				new Rental { Id = 3, CustomerId = 4, EquipmentId = 3, IssuedAt = today.AddDays(-5), DueDate = today.AddDays(2), Status = "Active" },
				new Rental { Id = 4, CustomerId = 5, EquipmentId = 4, IssuedAt = today.AddDays(-3), DueDate = today.AddDays(1), Status = "Active" },
				new Rental { Id = 5, CustomerId = 2, EquipmentId = 5, IssuedAt = today.AddDays(-10), DueDate = today.AddDays(-1), Status = "Active" }
			);

			modelBuilder.Entity<Equipment>().HasData(
				new Equipment { Id = 1, Name = "Excavator", Category = EquipmentCategory.HeavyMachinery, Condition = EquipmentCondition.Good, RentalPrice = 300.00M, Description = "Caterpillar excavator", IsAvailable = true, CreatedAt = DateTime.UtcNow.AddDays(-30) },
				new Equipment { Id = 2, Name = "Bulldozer", Category = EquipmentCategory.HeavyMachinery, Condition = EquipmentCondition.Fair, RentalPrice = 250.00M, Description = "Komatsu bulldozer", IsAvailable = true, CreatedAt = DateTime.UtcNow.AddDays(-28) },
				new Equipment { Id = 3, Name = "Power Drill", Category = EquipmentCategory.PowerTools, Condition = EquipmentCondition.Excellent, RentalPrice = 40.00M, Description = "Cordless drill", IsAvailable = false, CreatedAt = DateTime.UtcNow.AddDays(-20) },
				new Equipment { Id = 4, Name = "Pickup Truck", Category = EquipmentCategory.Vehicles, Condition = EquipmentCondition.Good, RentalPrice = 120.00M, Description = "3/4 ton pickup", IsAvailable = false, CreatedAt = DateTime.UtcNow.AddDays(-15) },
				new Equipment { Id = 5, Name = "Safety Harness", Category = EquipmentCategory.Safety, Condition = EquipmentCondition.New, RentalPrice = 15.00M, Description = "OSHA compliant", IsAvailable = false, CreatedAt = DateTime.UtcNow.AddDays(-10) }
			);
		}
	}
}
