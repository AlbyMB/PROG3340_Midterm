using PROG3340_MidtermProject_V3.Models.Domain;

namespace PROG3340_MidtermProject_V3.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<Customer?> AddAsync(Customer customer);
        Task<Customer?> GetByIdAsync(int id);
        Task<Customer?> UpdateAsync(Customer customer);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Customer>> GetAllAsync();

        Task<Customer?> GetByIdIncludeRentalsAsync(int id);
    }
}
