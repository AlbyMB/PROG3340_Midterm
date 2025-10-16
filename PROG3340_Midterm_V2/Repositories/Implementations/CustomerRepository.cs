using Microsoft.EntityFrameworkCore;
using PROG3340_MidtermProject_V2.Data;
using PROG3340_MidtermProject_V2.Models.Domain;
using PROG3340_MidtermProject_V2.Repositories.Interfaces;

namespace PROG3340_MidtermProject_V2.Unused
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;
        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<Customer?> AddAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            return customer;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.Customers.Include(c => c.Rentals).ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<Customer?> GetByIdIncludeRentalsAsync(int id)
        {
            return await _context.Customers.Include(c => c.Rentals).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Customer?> GetByUsernameAsync(string name)
        {
            return await _context.Customers.FirstOrDefaultAsync(u => u.Username == name);
        }

        public async Task<Customer?> UpdateAsync(Customer customer)
        {
            var customerToUpdate = await _context.Customers.FindAsync(customer.Id);
            if (customerToUpdate != null)
            {
                customerToUpdate.Username = customer.Username;
                customerToUpdate.Password = customer.Password;
                customerToUpdate.Role = customer.Role;
                return customerToUpdate;
            }
            return null;
        }
    }
}
