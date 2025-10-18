using PROG3340_Midterm.Repository.Interfaces;

namespace PROG3340_Midterm.UnitOfWork
{
	public interface IUnitOfWork
	{
		ICustomerRepository _customerRepository { get; }
		IEquipmentRepository _equipmentRepository { get; }
		IRentalRepository _rentalRepository { get; }
		bool Complete();
	}
}
