using PROG3340_Midterm.Data;
using PROG3340_Midterm.Repository.Interfaces;

namespace PROG3340_Midterm.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		public ICustomerRepository _customerRepository { get; }

		public IEquipmentRepository _equipmentRepository { get; }

		public IRentalRepository _rentalRepository { get; }

		private readonly AppDbContext _context;
		public UnitOfWork(ICustomerRepository customerRepository,
						IEquipmentRepository equipmentRepository,
						IRentalRepository rentalRepository,
						AppDbContext context)
		{ 
			_customerRepository = customerRepository;
			_equipmentRepository = equipmentRepository;
			_rentalRepository = rentalRepository;
			_context = context;
		}

		public bool Complete()
		{
			_context.SaveChanges();
			return true;
		}

	}
}
