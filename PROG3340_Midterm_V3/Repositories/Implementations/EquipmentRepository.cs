using Microsoft.EntityFrameworkCore;
using PROG3340_MidtermProject_V3.Data;
using PROG3340_MidtermProject_V3.Models.Domain;
using PROG3340_MidtermProject_V3.Repositories.Interfaces;

namespace PROG3340_MidtermProject_V3.Repositories.Implementations
{
    public class EquipmentRepository : IEquipmentRepository
    {
        private readonly AppDbContext _context;
        public EquipmentRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<Equipment?> AddAsync(Equipment equipment)
        {
            await _context.Equipment.AddAsync(equipment);
            return equipment;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment != null)
            {
                _context.Equipment.Remove(equipment);
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Equipment>> GetAllAsync()
        {
            return await _context.Equipment.Include(e => e.Rentals).ToListAsync();
        }

        public async Task<Equipment?> GetByIdAsync(int id)
        {
            return await _context.Equipment.FindAsync(id);
        }

        public async Task<Equipment?> UpdateAsync(Equipment equipment)
        {
            var equipmentToUpdate = await _context.Equipment.FindAsync(equipment.Id);
            if (equipmentToUpdate != null)
            {
                equipmentToUpdate.Name = equipment.Name;
                equipmentToUpdate.Category = equipment.Category;
                equipmentToUpdate.PriceRate = equipment.PriceRate;
                _context.Equipment.Update(equipmentToUpdate);
                return equipmentToUpdate;
            }
            return null;
        }


        public async Task<IEnumerable<Equipment>> GetAvailableEquipmentAsync()
        {
            return await _context.Equipment
                .Include(e => e.Rentals)
                .Where(e => e.IsAvailable)
                .ToListAsync();
        }

        public async Task<IEnumerable<Equipment>> GetUnavailableEquipmentAsync()
        {
            return await _context.Equipment
                .Include(e => e.Rentals)
                .Where(e => !e.IsAvailable)
                .ToListAsync();
        }
    }
}
