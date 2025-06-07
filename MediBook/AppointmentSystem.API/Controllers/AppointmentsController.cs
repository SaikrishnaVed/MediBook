using AutoMapper;
using MediBook.AppointmentSystem.API.Models;
using MediBook.AppointmentSystem.Core.Entities;
using MediBook.AppointmentSystem.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediBook.AppointmentSystem.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AppointmentsController : ControllerBase
	{
        private readonly IAppointmentService _service;
        private readonly IMapper _mapper;

        public AppointmentsController(IAppointmentService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Book([FromBody] AppointmentDto dto)
        {
            var appointment = _mapper.Map<Appointment>(dto);
            var result = await _service.BookAppointmentAsync(appointment);
            return Ok(new { message = result });
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAppointment([FromBody] UpdateAppointmentDto updatedAppointment)
        {
            if (updatedAppointment == null || updatedAppointment.AppointmentId <= 0)
                return BadRequest("Invalid appointment data.");

            var result = await _service.UpdateAppointmentAsync(updatedAppointment);

            if (!result)
                return NotFound("Appointment not found.");

            return Ok("Appointment updated successfully.");
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            var result = await _service.CancelAppointmentAsync(id);
            return result ? Ok() : NotFound();
        }

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatient(int patientId, int? doctorId)
        {
            var appointments = await _service.GetAppointmentsByPatientId(patientId);
            return Ok(appointments);
        }
    }
}
