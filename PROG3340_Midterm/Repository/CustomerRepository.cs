using Microsoft.EntityFrameworkCore;
using PROG3340_Midterm.Data;
using PROG3340_Midterm.Models;
using PROG3340_Midterm.Repository.Interfaces;

namespace PROG3340_Midterm.Repository
{
	public class CustomerRepository : ICustomerRepository
	{
		private readonly AppDbContext _context;
		public CustomerRepository(AppDbContext context)
		{
			_context = context;
		}


		public Customer? Add(Customer customer)
		{
			_context.Customers.Add(customer);
			return customer;
		}

		public bool Delete(int id)
		{
			var customer = _context.Customers.Find(id);
			if (customer != null)
			{
				_context.Customers.Remove(customer);
				return true;
			}
			return false;
		}

		public IEnumerable<Customer> GetAll()
		{
			return _context.Customers.Include(c => c.Rentals).ToList();
		}

		public Customer? GetById(int id)
		{
			return _context.Customers.Find(id);
		}

		public Customer? GetByIdIncludeRentals(int id)
		{
			return _context.Customers.Include(c => c.Rentals).FirstOrDefault(c => c.Id == id);
		}

		public Customer? GetByUsername(string name)
		{
			return _context.Customers.FirstOrDefault(u => u.UserName == name);
		}

		public Customer? Update(Customer customer)
		{
			var customerToUpdate = _context.Customers.Find(customer.Id);
			if (customerToUpdate != null)
			{
				customerToUpdate.UserName = customer.UserName;
				customerToUpdate.Password = customer.Password;
				customerToUpdate.Role = customer.Role;
				return customerToUpdate;
			}
			return null;
		}
	}
}
