using Microsoft.EntityFrameworkCore;
using PROG3340_Midterm.Data;
using PROG3340_Midterm.Models;
using PROG3340_Midterm.Repository.Interfaces;

namespace PROG3340_Midterm.Repository
{
	public class RentalRepository : IRentalRepository
	{
		private readonly AppDbContext _context;
		public RentalRepository(AppDbContext context)
		{
			_context = context;
		}


		public Rental? Issue(Rental rental)
		{
			_context.Rentals.Add(rental);
			return rental;
		}

		public bool Delete(int id)
		{
			var rental = _context.Rentals.Find(id);
			if (rental != null)
			{
				_context.Rentals.Remove(rental);
				return true;
			}
			return false;
		}

		public IEnumerable<Rental> GetActiveRentals()
		{
			return _context.Rentals.Where(r => r.ReturnedAt == null).Include(r => r.Equipment).Include(r => r.Customer).ToList();
		}

		public IEnumerable<Rental> GetAll()
		{
			return _context.Rentals.Include(r => r.Equipment).Include(r => r.Customer).ToList();
		}

		public IEnumerable<Rental> GetAllByEquipmentId(int equipmentId)
		{
			return _context.Rentals.Where(r => r.EquipmentId == equipmentId).Include(r => r.Equipment).ToList();
		}

		public Rental? GetById(int id)
		{
			return _context.Rentals.Find(id);
		}

		public IEnumerable<Rental> GetCompletedRentals()
		{
			return _context.Rentals.Where(r => r.ReturnedAt != null).Include(r => r.Equipment).Include(r => r.Customer).ToList();
		}

		public IEnumerable<Rental> GetOverdueRentals()
		{
			return _context.Rentals.Where(r => r.DueDate < DateTime.Today).Include(r => r.Equipment).Include(r => r.Customer).ToList();
		}

		public Rental? Return(Rental rental)
		{
			var rentalToUpdate = _context.Rentals.Find(rental.Id);
			if (rentalToUpdate != null)
			{
				rentalToUpdate.ReturnedAt = rental.ReturnedAt;
				rentalToUpdate.ReturnCondition = rental.ReturnCondition;
				rentalToUpdate.Notes = rental.Notes;
				_context.Rentals.Update(rentalToUpdate);
				return rentalToUpdate;
			}
			return null;
		}

		public Rental? ExtendRental(Rental rental)
		{
			var rentalToUpdate = _context.Rentals.Find(rental.Id);
			if (rentalToUpdate != null)
			{
				rentalToUpdate.DueDate = rental.DueDate;
				rentalToUpdate.ExtensionReason = rental.ExtensionReason;
				_context.Rentals.Update(rentalToUpdate);
				return rentalToUpdate;
			}
			return null;
		}
	}
}
