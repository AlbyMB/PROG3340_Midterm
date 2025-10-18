using PROG3340_MidtermProject_V3.Services.Interfaces;

namespace PROG3340_MidtermProject_V3.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
		ICustomerService _customerService { get; }
		IEquipmentService _equipmentService { get; }
		IRentalService _rentalService { get; }
		Task<bool> CompleteAsync();
    }
}
