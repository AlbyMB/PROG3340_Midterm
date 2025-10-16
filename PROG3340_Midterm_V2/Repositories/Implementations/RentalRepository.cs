using Microsoft.EntityFrameworkCore;
using PROG3340_MidtermProject_V2.Data;
using PROG3340_MidtermProject_V2.Models.Domain;
using PROG3340_MidtermProject_V2.Repositories.Interfaces;

namespace PROG3340_MidtermProject_V2.Repositories.Implementations
{
    public class RentalRepository : IRentalRepository
    {
        private readonly AppDbContext _context;
        public RentalRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<Rental?> AddAsync(Rental rental)
        {
            await _context.Rentals.AddAsync(rental);
            return rental;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var rental = await _context.Rentals.FindAsync(id);
            if (rental != null)
            {
                _context.Rentals.Remove(rental);
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Rental>> GetActiveRentalsAsync()
        {
            return await _context.Rentals.Where(r => r.ReturnDate == null).Include(r => r.Equipment).Include(r => r.Customer).ToListAsync();
        }

        public async Task<IEnumerable<Rental>> GetAllAsync()
        {
            return await _context.Rentals.Include(r => r.Equipment).Include(r => r.Customer).ToListAsync();
        }

        public async Task<IEnumerable<Rental>> GetAllByEquipmentIdAsync(int equipmentId)
        {
            return await _context.Rentals.Where(r => r.EquipmentId == equipmentId).Include(r => r.Equipment).ToListAsync();
        }

        public async Task<Rental?> GetByIdAsync(int id)
        {
            return await _context.Rentals.FindAsync(id);
        }

        public async Task<IEnumerable<Rental>> GetCompletedRentalsAsync()
        {
            return await _context.Rentals.Where(r => r.ReturnDate != null).Include(r => r.Equipment).Include(r => r.Customer).ToListAsync();
        }

        public async Task<IEnumerable<Rental>> GetOverdueRentalsAsync()
        {
            return await _context.Rentals.Where(r => r.IsOverdue).Include(r => r.Equipment).Include(r => r.Customer).ToListAsync();
        }

        public async Task<Rental?> ReturnAsync(Rental rental)
        {
            var rentalToUpdate = await _context.Rentals.FindAsync(rental.Id);
            if (rentalToUpdate != null)
            {
                rentalToUpdate.ReturnDate = rental.ReturnDate;
                rentalToUpdate.Condition = rental.Condition;
                rentalToUpdate.Notes = rental.Notes;
                _context.Rentals.Update(rentalToUpdate);
                return rentalToUpdate;
            }
            return null;
        }

        public async Task<Rental?> ExtendRentalAsync(Rental rental)
        {
            var rentalToUpdate = await _context.Rentals.FindAsync(rental.Id);
            if (rentalToUpdate != null)
            {
                rentalToUpdate.DueDate = rental.DueDate;
                rentalToUpdate.ExtentionReason = rental.ExtentionReason;
                _context.Rentals.Update(rentalToUpdate);
                return rentalToUpdate;
            }
            return null;
        }
    }
}
