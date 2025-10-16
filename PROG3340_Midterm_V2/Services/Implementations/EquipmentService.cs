using PROG3340_MidtermProject_V2.Models.Domain;
using PROG3340_MidtermProject_V2.Repositories.Interfaces;
using PROG3340_MidtermProject_V2.Services.Interfaces;

namespace PROG3340_MidtermProject_V2.Services.Implementations
{
    public class EquipmentService : IEquipmentService
    {
        private readonly IEquipmentRepository _repository;
        public EquipmentService(IEquipmentRepository repository)
        {
            _repository = repository;
        }


        // Admin
        public async Task<Equipment?> AddAsync(Equipment equipment)
        {
            return await _repository.AddAsync(equipment);
        }

        // Admin
        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        // All
        public async Task<IEnumerable<Equipment>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        // All
        public async Task<IEnumerable<Equipment>> GetAvailableEquipmentAsync()
        {
            return await _repository.GetAvailableEquipmentAsync();
        }

        // All
        public async Task<Equipment?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        // All
        public async Task<IEnumerable<Equipment>> GetUnavailableEquipmentAsync()
        {
            return await _repository.GetUnavailableEquipmentAsync();
        }

        // Admin
        public async Task<Equipment?> UpdateAsync(Equipment equipment)
        {
            return await _repository.UpdateAsync(equipment);
        }
    }
}
