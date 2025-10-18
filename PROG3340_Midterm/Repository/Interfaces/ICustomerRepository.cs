using PROG3340_Midterm.Models;

namespace PROG3340_Midterm.Repository.Interfaces
{
	public interface ICustomerRepository
	{
		Customer? Add(Customer customer);
		Customer? GetById(int id);
		Customer? Update(Customer customer);
		bool Delete(int id);
		IEnumerable<Customer> GetAll();

		Customer? GetByIdIncludeRentals(int id);
	}
}
