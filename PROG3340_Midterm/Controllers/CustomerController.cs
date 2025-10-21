using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PROG3340_Midterm.Models;
using PROG3340_Midterm.UnitOfWork;
using System.Security.Claims;

namespace PROG3340_Midterm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        
		[Authorize(Roles = "Admin")]
        [HttpGet]
		public IActionResult GetAllCustomers()
        {
            var customers = _unitOfWork._customerRepository.GetAll();
            return Ok(customers);
        }

        
		[Authorize(Roles = "Admin,User")]
        [HttpGet("{id}")]
		public IActionResult GetCustomer(int id)
        {
            var customer = _unitOfWork._customerRepository.GetByIdIncludeRentals(id);
            if(customer == null)
                return NotFound();

			var (role, userId) = GetUserInfo();

			if (role == null || userId == null)
				return Unauthorized();

			if (role == "Admin")
				return Ok(customer);

			if (customer.Id != userId)
				return NotFound();

            return Ok(customer);
        }

        
		[Authorize(Roles = "Admin")]
        [HttpPost]
		public IActionResult CreateCustomer([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newCustomer = _unitOfWork._customerRepository.Add(customer);
            if (newCustomer == null)
                return BadRequest("Failed to create customer");

            _unitOfWork.Complete();
            return CreatedAtAction(nameof(GetCustomer), new { id = newCustomer.Id }, newCustomer);
        }

        
        [Authorize(Roles = "Admin,User")]
        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(int id, [FromBody] Customer customer)
        {
            if (id != customer.Id)
                return BadRequest("ID mismatch");

            var existingCustomer = _unitOfWork._customerRepository.GetById(id);
            if (existingCustomer == null)
                return NotFound();

            var (role, userId) = GetUserInfo();
            if (role == null || userId == null)
                return Unauthorized();

            // Users can only update their own profile; Admin can update any
            var isAdmin = role == "Admin";
            var isSelf = userId == id;
            if (!isAdmin && !isSelf)
                return Forbid();

            // Admin cannot change another user's username or password
            if (isAdmin && !isSelf)
            {
                customer.UserName = existingCustomer.UserName;
                customer.Password = existingCustomer.Password;
            }

            var updatedCustomer = _unitOfWork._customerRepository.Update(customer);
            if (updatedCustomer == null)
                return BadRequest("Failed to update customer");

            _unitOfWork.Complete();
            return Ok(updatedCustomer);
        }

        
		[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
		public IActionResult DeleteCustomer(int id)
        {
            var customer = _unitOfWork._customerRepository.GetById(id);
            if (customer == null)
                return NotFound();

            if (_unitOfWork._customerRepository.Delete(id))
            {
                _unitOfWork.Complete();
                return NoContent();
            }

            return BadRequest("Failed to delete customer");
        }

        
        [Authorize(Roles = "Admin,User")]
        [HttpGet("{id}/rentals")]
        public IActionResult GetCustomerRentals(int id)
        {
            var customer = _unitOfWork._customerRepository.GetByIdIncludeRentals(id);
            if (customer == null)
                return NotFound();

			var (role, userId) = GetUserInfo();

			if (role == null || userId == null)
				return Unauthorized();

			if (role == "Admin")
				return Ok(customer.Rentals);

			if (customer.Id != userId)
				return NotFound();

			return Ok(customer.Rentals);
        }

        
        [Authorize(Roles = "Admin,User")]
        [HttpGet("{id}/active-rental")]
        public IActionResult GetCustomerActiveRental(int id)
        {
            var customer = _unitOfWork._customerRepository.GetByIdIncludeRentals(id);
            if (customer == null)
                return NotFound();

			var (role, userId) = GetUserInfo();

			if (role == null || userId == null)
				return Unauthorized();

			var activeRental = customer.Rentals?
                .FirstOrDefault(r => r.ReturnedAt == null && r.Status == "Active");

            if (activeRental == null)
                return NotFound("No active rental found");

			if (role == "Admin")
				return Ok(activeRental);

			if (customer.Id != userId)
				return NotFound();


            return Ok(activeRental);
        }


		private (string? role, int? userId) GetUserInfo()
		{
			var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
			var userIdString = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			if (string.IsNullOrEmpty(userRole) || string.IsNullOrEmpty(userIdString))
				return (null, null);

			return (userRole, int.Parse(userIdString));
		}

		private IEnumerable<Rental> FilterRentalsByUserRole(IEnumerable<Rental> rentals)
		{
			var (role, userId) = GetUserInfo();

			if (role == null || userId == null)
				return Enumerable.Empty<Rental>();

			if (role != "Admin")
				return rentals.Where(r => r.CustomerId == userId);

			return rentals;
		}
	}
}
