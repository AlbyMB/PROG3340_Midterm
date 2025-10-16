using PROG3340_MidtermProject_V2.Models.Domain;

namespace PROG3340_MidtermProject_V2.Services.Interfaces
{
    public interface IRentalService
    {
        Task<IEnumerable<Rental>> GetAllAsync();
        Task<Rental?> GetByIdAsync(int id);
        Task<Rental?> AddAsync(Rental rental);
        Task<Rental?> ExtendRentalAsync(Rental rental);
        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<Rental>> GetOverdueRentalsAsync();
        Task<IEnumerable<Rental>> GetActiveRentalsAsync();
        Task<IEnumerable<Rental>> GetCompletedRentalsAsync();

        Task<IEnumerable<Rental>> GetAllByEquipmentIdAsync(int equipmentId);
    }
}
