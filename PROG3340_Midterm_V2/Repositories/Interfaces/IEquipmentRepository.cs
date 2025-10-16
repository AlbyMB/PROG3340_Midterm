using PROG3340_MidtermProject_V2.Models.Domain;

namespace PROG3340_MidtermProject_V2.Repositories.Interfaces
{
    public interface IEquipmentRepository
    {
        Task<IEnumerable<Equipment>> GetAllAsync();
        Task<Equipment?> GetByIdAsync(int id);
        Task<Equipment?> AddAsync(Equipment equipment);
        Task<Equipment?> UpdateAsync(Equipment equipment);
        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<Equipment>> GetAvailableEquipmentAsync();
        Task<IEnumerable<Equipment>> GetUnavailableEquipmentAsync();

    }
}
