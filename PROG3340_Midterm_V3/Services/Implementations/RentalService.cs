using PROG3340_MidtermProject_V3.Models.Domain;
using PROG3340_MidtermProject_V3.Repositories.Interfaces;
using PROG3340_MidtermProject_V3.Services.Interfaces;
using System.Security.Claims;

namespace PROG3340_MidtermProject_V3.Services.Implementations
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User; // Fetch claims
        private bool IsAdmin => User?.IsInRole("Admin") ?? false; // Is admin
        private int? UserId => int.TryParse(User?.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : null; // Get user ID
        public RentalService(IRentalRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Rental?> AddAsync(Rental rental) // Issue Rental
        {
            if (IsAdmin || UserId == rental.CustomerId)
            {
                return await _repository.AddAsync(rental);
            }
            return null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<Rental?> ExtendRentalAsync(Rental rental)
        {
            return await _repository.ExtendRentalAsync(rental);
        }

        public async Task<IEnumerable<Rental>> GetActiveRentalsAsync()
        {
            if (IsAdmin)
            {
                return await _repository.GetActiveRentalsAsync();
            }
            var allRentals = await _repository.GetActiveRentalsAsync();
            return allRentals.Where(r => r.CustomerId == UserId);
        }

        public async Task<IEnumerable<Rental>> GetAllAsync()
        {
            if (IsAdmin)
            {
                return await _repository.GetAllAsync();
            }
            var allRentals = await _repository.GetAllAsync();
            return allRentals.Where(r => r.CustomerId == UserId);
        }

        public async Task<IEnumerable<Rental>> GetAllByEquipmentIdAsync(int equipmentId)
        {
            return await _repository.GetAllByEquipmentIdAsync(equipmentId);
        }

        public async Task<Rental?> GetByIdAsync(int id)
        {
            if (IsAdmin || id == UserId)
            {
                return await _repository.GetByIdAsync(id);
            }
            return null;
        }

        public async Task<IEnumerable<Rental>> GetCompletedRentalsAsync()
        {
            if (IsAdmin)
            {
                return await _repository.GetCompletedRentalsAsync();
            }
            var allRentals = await _repository.GetCompletedRentalsAsync();
            return allRentals.Where(r => r.CustomerId == UserId);
        }

        public async Task<IEnumerable<Rental>> GetOverdueRentalsAsync()
        {
            if (IsAdmin)
            {
                return await _repository.GetOverdueRentalsAsync();
            }
            var allRentals = await _repository.GetOverdueRentalsAsync();
            return allRentals.Where(r => r.CustomerId == UserId);
        }
    }
}
