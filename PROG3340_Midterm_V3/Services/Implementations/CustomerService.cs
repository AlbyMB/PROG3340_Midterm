using PROG3340_MidtermProject_V3.Models.Domain;
using PROG3340_MidtermProject_V3.Repositories.Interfaces;
using PROG3340_MidtermProject_V3.Services.Interfaces;
using System.Security.Claims;

namespace PROG3340_MidtermProject_V3.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User; // Fetch claims
        private bool IsAdmin => User?.IsInRole("Admin") ?? false; // Is admin
        private int? UserId => int.TryParse(User?.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : null; // Get user ID
        public CustomerService(ICustomerRepository repository, IHttpContextAccessor accessor)
        {
            _repository = repository;
            _httpContextAccessor = accessor;
        }

        // Admin
        public async Task<Customer?> AddAsync(Customer customer)
        {
            return await _repository.AddAsync(customer);
        }

        // Admin
        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        // Admin
        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        // Admin or Self
        public async Task<Customer?> GetByIdAsync(int id)
        {
            if (IsAdmin || UserId == id)
            {
                return await _repository.GetByIdAsync(id);
            }
            return null;
        }

        // Admin or Self
        public async Task<Customer?> GetByIdIncludeRentalsAsync(int id)
        {
            if (IsAdmin || UserId == id)
            {
                return await _repository.GetByIdIncludeRentalsAsync(id);
            }
            return null;
        }

        // Admin or Self
        public async Task<Customer?> UpdateAsync(Customer customer)
        {
            if (IsAdmin || UserId == customer.Id)
            {
                return await _repository.UpdateAsync(customer);
            }
            return null;
        }
    }
}
