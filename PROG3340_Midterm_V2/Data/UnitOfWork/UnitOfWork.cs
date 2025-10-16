
using PROG3340_MidtermProject_V2.Services.Interfaces;

namespace PROG3340_MidtermProject_V2.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool disposedValue;
        private readonly ICustomerService _customerService;
        private readonly IEquipmentService _equipmentService;
        private readonly IRentalService _rentalService;
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
