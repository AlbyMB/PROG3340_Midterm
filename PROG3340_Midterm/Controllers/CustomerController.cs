using Microsoft.AspNetCore.Mvc;
using PROG3340_Midterm.Models;
using PROG3340_Midterm.UnitOfWork;

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

        // GET: api/customers
        [HttpGet]
        public IActionResult GetAllCustomers()
        {
            var customers = _unitOfWork._customerRepository.GetAll();
            return Ok(customers);
        }

        // GET: api/customers/{id}
        [HttpGet("{id}")]
        public IActionResult GetCustomer(int id)
        {
            var customer = _unitOfWork._customerRepository.GetByIdIncludeRentals(id);
            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        // POST: api/customers
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

        // PUT: api/customers/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(int id, [FromBody] Customer customer)
        {
            if (id != customer.Id)
                return BadRequest("ID mismatch");

            var existingCustomer = _unitOfWork._customerRepository.GetById(id);
            if (existingCustomer == null)
                return NotFound();

            var updatedCustomer = _unitOfWork._customerRepository.Update(customer);
            if (updatedCustomer == null)
                return BadRequest("Failed to update customer");

            _unitOfWork.Complete();
            return Ok(updatedCustomer);
        }

        // DELETE: api/customers/{id}
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

        // GET: api/customers/{id}/rentals
        [HttpGet("{id}/rentals")]
        public IActionResult GetCustomerRentals(int id)
        {
            var customer = _unitOfWork._customerRepository.GetByIdIncludeRentals(id);
            if (customer == null)
                return NotFound();

            return Ok(customer.Rentals);
        }

        // GET: api/customers/{id}/active-rental
        [HttpGet("{id}/active-rental")]
        public IActionResult GetCustomerActiveRental(int id)
        {
            var customer = _unitOfWork._customerRepository.GetByIdIncludeRentals(id);
            if (customer == null)
                return NotFound();

            var activeRental = customer.Rentals?
                .FirstOrDefault(r => r.ReturnedAt == null && r.Status == "Active");

            if (activeRental == null)
                return NotFound("No active rental found");

            return Ok(activeRental);
        }
    }
}
