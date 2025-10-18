using PROG3340_Midterm.Models;

namespace PROG3340_Midterm.Repository.Interfaces
{
	public interface IEquipmentRepository
	{
		IEnumerable<Equipment> GetAll();
		Equipment GetById(int id);
		Equipment Add(Equipment equipment);
		Equipment Update(Equipment equipment);
		bool Delete(int id);
		IEnumerable<Equipment> GetAvailableEquipment();
		IEnumerable<Equipment> GetUnavailableEquipment();
	}
}
