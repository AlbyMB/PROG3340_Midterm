using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PROG3340_Midterm.Models;
using PROG3340_Midterm.UnitOfWork;
using System.Linq;
using System.Security.Claims;

namespace PROG3340_Midterm.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RentalController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;

		public RentalController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		[Authorize(Roles = "Admin,User")]
		[HttpGet]
		public IActionResult GetAllRentals()
		{
			var rentals = _unitOfWork._rentalRepository.GetAll();
			var filteredRentals = FilterRentalsByUserRole(rentals);
			
			if (!filteredRentals.Any())
				return Unauthorized();
				
			return Ok(filteredRentals);
		}

		[Authorize(Roles = "Admin,User")]
		[HttpGet("{id}")]
		public IActionResult GetRentalById(int id)
		{
			var rental = _unitOfWork._rentalRepository.GetById(id);
			if (rental == null)
			{
				return NotFound();
			}

			var (role, userId) = GetUserInfo();

			if (role == null || userId == null)
				return Unauthorized();

			if (role == "Admin")
				return Ok(rental);

			if (rental.CustomerId != userId)
				return NotFound();

			return Ok(rental);
		}

		[Authorize(Roles = "Admin,User")]
		[HttpPost("issue")]
		public IActionResult IssueEquipment([FromBody] Rental rental)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var (role, userId) = GetUserInfo();
			if (role == null || userId == null)
				return Unauthorized();

			if(rental.CustomerId != userId)
				return BadRequest(ModelState);

			var equipment = _unitOfWork._equipmentRepository.GetById(rental.EquipmentId);
			if (equipment == null)
				return NotFound("Equipment not found");

			var customer = _unitOfWork._customerRepository.GetById(rental.CustomerId);
			if (customer == null)
				return NotFound("Customer not found");

			var issuedRental = _unitOfWork._rentalRepository.Issue(rental);
			if (issuedRental == null)
				return BadRequest("Failed to issue rental");

			_unitOfWork.Complete();
			return CreatedAtAction(nameof(GetRentalById), new { id = issuedRental.Id }, issuedRental);
		}

		[Authorize(Roles = "Admin,User")]
		[HttpPost("return")]
		public IActionResult ReturnEquipment([FromBody] Rental rental)
		{
			var existingRental = _unitOfWork._rentalRepository.GetById(rental.Id);
			if (existingRental == null)
				return NotFound();

			var (role, userId) = GetUserInfo();
			if (role == null || userId == null)
				return Unauthorized();

			if (rental.CustomerId != userId)
				return BadRequest(ModelState);

			existingRental.ReturnedAt = DateTime.Now;
			existingRental.ReturnCondition = rental.ReturnCondition;
			existingRental.Status = "Completed";
			existingRental.Notes = rental.Notes;

			_unitOfWork.Complete();
			return Ok(existingRental);
		}

		[Authorize(Roles = "Admin,User")]
		[HttpGet("active")]
		public IActionResult GetActiveRentals()
		{
			var activeRentals = _unitOfWork._rentalRepository.GetActiveRentals();
			var filteredRentals = FilterRentalsByUserRole(activeRentals);

			return Ok(filteredRentals);
		}

		[Authorize(Roles = "Admin,User")]
		[HttpGet("completed")]
		public IActionResult GetCompletedRentals()
		{
			var completedRentals = _unitOfWork._rentalRepository.GetCompletedRentals();
			var filteredRentals = FilterRentalsByUserRole(completedRentals);
			return Ok(filteredRentals);
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("overdue")]
		public IActionResult GetOverdueRentals()
		{
			var overdueRentals = _unitOfWork._rentalRepository.GetOverdueRentals();
			return Ok(overdueRentals);
		}

		[Authorize(Roles = "Admin,User")]
		[HttpGet("equipment/{equipmentId}")]
		public IActionResult GetRentalsByEquipment(int equipmentId)
		{
			var equipment = _unitOfWork._equipmentRepository.GetById(equipmentId);
			if (equipment == null)
				return NotFound("Equipment not found");

			var rentals = _unitOfWork._rentalRepository.GetAllByEquipmentId(equipmentId);
			return Ok(rentals);
		}

		[Authorize(Roles = "Admin")]
		[HttpPut("{id}")]
		public IActionResult ExtendRental(int id, [FromBody] Rental rental)
		{
			if (id != rental.Id)
				return BadRequest("ID mismatch");

			var existingRental = _unitOfWork._rentalRepository.GetById(id);
			if (existingRental == null)
				return NotFound();

			var updatedRental = _unitOfWork._rentalRepository.ExtendRental(rental);
			if (updatedRental == null)
				return BadRequest("Failed to extend rental");

			_unitOfWork.Complete();
			return Ok(updatedRental);
		}

		[Authorize(Roles = "Admin")]
		[HttpDelete("{id}")]
		public IActionResult CancelRental(int id)
		{
			var rental = _unitOfWork._rentalRepository.GetById(id);
			if (rental == null)
				return NotFound();

			if (_unitOfWork._rentalRepository.Delete(id))
			{
				_unitOfWork.Complete();
				return NoContent();
			}

			return BadRequest("Failed to cancel rental");
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
