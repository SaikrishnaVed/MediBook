using MediBook.AppointmentSystem.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediBook.AppointmentSystem.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DoctorsController : ControllerBase
	{
        private readonly IDoctorService _service;
        public DoctorsController(IDoctorService service) => _service = service;

        [HttpGet]
        public IActionResult GetBySpecialty([FromQuery] string specialty)
        {
            var doctors = _service.GetDoctorsBySpecialty(specialty);
            return Ok(doctors);
        }
    }
}