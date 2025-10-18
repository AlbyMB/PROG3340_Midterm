
using PROG3340_MidtermProject_V3.Services.Interfaces;

namespace PROG3340_MidtermProject_V3.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool disposedValue;
        public  ICustomerService _customerService { get; }
        public IEquipmentService _equipmentService { get; }
        public IRentalService _rentalService { get; }
        private readonly AppDbContext _context;
        public UnitOfWork(ICustomerService customerService, IEquipmentService equipmentService, IRentalService rentalService, AppDbContext context)
        {
            _customerService = customerService;
            _equipmentService = equipmentService;
            _rentalService = rentalService;
            _context = context;
        }

        public async Task<bool> CompleteAsync()
        {
            await _context.SaveChangesAsync();
            Dispose();
            return true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
