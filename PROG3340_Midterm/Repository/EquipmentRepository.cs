using Microsoft.EntityFrameworkCore;
using PROG3340_Midterm.Data;
using PROG3340_Midterm.Models;
using PROG3340_Midterm.Repository.Interfaces;

namespace PROG3340_Midterm.Repository
{
	public class EquipmentRepository : IEquipmentRepository
	{
		private readonly AppDbContext _context;
		public EquipmentRepository(AppDbContext context)
		{
			_context = context;
		}


		public Equipment? Add(Equipment equipment)
		{
			_context.Equipments.Add(equipment);
			return equipment;
		}

		public bool Delete(int id)
		{
			var equipment = _context.Equipments.Find(id);
			if (equipment != null)
			{
				_context.Equipments.Remove(equipment);
				return true;
			}
			return false;
		}

		public IEnumerable<Equipment> GetAll()
		{
			return _context.Equipments.Include(e => e.Rentals).ToList();
		}

		public Equipment? GetById(int id)
		{
			return _context.Equipments.Find(id);
		}

		public Equipment? Update(Equipment equipment)
		{
			var equipmentToUpdate = _context.Equipments.Find(equipment.Id);
			if (equipmentToUpdate != null)
			{
				equipmentToUpdate.Name = equipment.Name;
				equipmentToUpdate.Category = equipment.Category;
				equipmentToUpdate.RentalPrice = equipment.RentalPrice;
				_context.Equipments.Update(equipmentToUpdate);
				return equipmentToUpdate;
			}
			return null;
		}


		public IEnumerable<Equipment> GetAvailableEquipment()
		{
			return _context.Equipments
				.Include(e => e.Rentals)
				.Where(e => e.IsAvailable)
				.ToList();
		}

		public IEnumerable<Equipment> GetUnavailableEquipment()
		{
			return _context.Equipments
				.Include(e => e.Rentals)
				.Where(e => !e.IsAvailable)
				.ToList();
		}
	}
}
