using PROG3340_Midterm.Models;

namespace PROG3340_Midterm.Repository.Interfaces
{
	public interface IRentalRepository
	{
		IEnumerable<Rental> GetAll();
		Rental? GetById(int id);
		Rental? Issue(Rental rental);
		Rental? ExtendRental(Rental rental);
		bool Delete(int id);

		IEnumerable<Rental> GetOverdueRentals();
		IEnumerable<Rental> GetActiveRentals();
		IEnumerable<Rental> GetCompletedRentals();

		IEnumerable<Rental> GetAllByEquipmentId(int equipmentId);
	}
}
