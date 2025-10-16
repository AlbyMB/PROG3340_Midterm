using Microsoft.EntityFrameworkCore;
using PROG3340_MidtermProject_V2.Models.Domain;
using PROG3340_MidtermProject_V2.Unused;

namespace PROG3340_MidtermProject_V2.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<Rental> Rentals { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder) // TODO: Finish seeding data
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<Customer>().HasData(
        //        new Customer { Id = 1, Username = "admin", Password = "admin123", Role = "Admin" },
        //        new Customer { Id = 2, Username = "user", Password = "user123", Role = "User" }
        //    );

        //    modelBuilder.Entity<Equipment>().HasData(
        //        new Equipment { Id = 1, Name = "Excavator", Category = "Heavy Machinery", PriceRate = 300.00M },
        //        new Equipment { Id = 2, Name = "Bulldozer", Category = "Heavy Machinery", PriceRate = 250.00M }
        //    );

        //    modelBuilder.Entity<Rental>().HasData(
        //        // Past rentals
        //        new Rental { Id = 1, CustomerId = 1, EquipmentId = 1, RentalDate = DateTime.Today.AddDays(-10), DueDate = DateTime.Today.AddDays(-5) },
        //        new Rental { Id = 2, CustomerId = 2, EquipmentId = 2, RentalDate = DateTime.Today.AddDays(-8), DueDate = DateTime.Today.AddDays(-3) },
        //        // Current rentals
        //        new Rental { Id = 3, CustomerId = 1, EquipmentId = 2, RentalDate = DateTime.Today.AddDays(-2), DueDate = DateTime.Today.AddDays(3) },
        //        // Future rentals
        //        new Rental { Id = 4, CustomerId = 2, EquipmentId = 1, RentalDate = DateTime.Today.AddDays(5), DueDate = DateTime.Today.AddDays(10) }
        //    );
        //}
    }
}
