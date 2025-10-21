using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PROG3340_Midterm.Models;
using PROG3340_Midterm.UnitOfWork;

namespace PROG3340_Midterm.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EquipmentController : ControllerBase
	{
        private readonly IUnitOfWork _unitOfWork;

        public EquipmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

		[Authorize(Roles = "Admin,User")]
        [HttpGet]
		public IActionResult GetAllEquipment()
        {
            var equipment = _unitOfWork._equipmentRepository.GetAll();
            return Ok(equipment);
        }


		[Authorize(Roles = "Admin,User")]
        [HttpGet("{id}")]
		public IActionResult GetEquipment(int id)
        {
            var equipment = _unitOfWork._equipmentRepository.GetById(id);
            if (equipment == null)
                return NotFound();

            
            var rentalHistory = _unitOfWork._rentalRepository.GetAllByEquipmentId(id);
            var response = new
            {
                Equipment = equipment,
                RentalHistory = rentalHistory
            };

            return Ok(response);
        }

		[Authorize(Roles = "Admin")]
		[HttpPost]
        public IActionResult CreateEquipment([FromBody] Equipment equipment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newEquipment = _unitOfWork._equipmentRepository.Add(equipment);
            if (newEquipment == null)
                return BadRequest("Failed to create equipment");

            _unitOfWork.Complete();
            return CreatedAtAction(nameof(GetEquipment), new { id = newEquipment.Id }, newEquipment);
        }

        
		[Authorize(Roles = "Admin")]
		[HttpPut("{id}")]
        public IActionResult UpdateEquipment(int id, [FromBody] Equipment equipment)
        {
            if (id != equipment.Id)
                return BadRequest("ID mismatch");

            var existingEquipment = _unitOfWork._equipmentRepository.GetById(id);
            if (existingEquipment == null)
                return NotFound();

            var updatedEquipment = _unitOfWork._equipmentRepository.Update(equipment);
            if (updatedEquipment == null)
                return BadRequest("Failed to update equipment");

            _unitOfWork.Complete();
            return Ok(updatedEquipment);
        }

        
		[Authorize(Roles = "Admin")]
		[HttpDelete("{id}")]
        public IActionResult DeleteEquipment(int id)
        {
            var equipment = _unitOfWork._equipmentRepository.GetById(id);
            if (equipment == null)
                return NotFound();

            // Disallow delete if any rentals exist (active or historical) to avoid FK constraint errors
            var hasAnyRentals = _unitOfWork._rentalRepository.GetAllByEquipmentId(id).Any();
            if (hasAnyRentals)
                return BadRequest("Cannot delete equipment with rental history. Remove related rentals first or deactivate equipment.");

            if (_unitOfWork._equipmentRepository.Delete(id))
            {
                _unitOfWork.Complete();
                return NoContent();
            }

            return BadRequest("Failed to delete equipment");
        }


		[Authorize(Roles = "Admin,User")]
        [HttpGet("available")]
		public IActionResult GetAvailableEquipment()
        {
            var availableEquipment = _unitOfWork._equipmentRepository.GetAvailableEquipment();
            return Ok(availableEquipment);
        }

        
		[Authorize(Roles = "Admin")]
		[HttpGet("rented")]
        public IActionResult GetRentedEquipment()
        {
            var rentedEquipment = _unitOfWork._equipmentRepository.GetUnavailableEquipment();
            
            var response = new
            {
                Count = rentedEquipment.Count(),
                Equipment = rentedEquipment
            };

            return Ok(response);
        }
    }
}
