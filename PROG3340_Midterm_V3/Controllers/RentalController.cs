using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PROG3340_MidtermProject_V3.Data.UnitOfWork;

namespace PROG3340_Midterm_V3.Controllers
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

		[Authorize(Roles = "Admin")]
		[HttpGet]
		public async Task<IActionResult> GetAllRentals()
		{
			var rentals = await _unitOfWork._rentalService.GetAllAsync();
			return Ok(rentals);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetRentalById(int id)
		{
			var rental = await _unitOfWork._rentalService.GetByIdAsync(id);
			if (rental == null)
			{
				return NotFound();
			}
			return Ok(rental);
		}


	}
}
