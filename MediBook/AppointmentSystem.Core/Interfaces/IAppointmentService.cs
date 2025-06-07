using MediBook.AppointmentSystem.API.Models;
using MediBook.AppointmentSystem.Core.Entities;

namespace MediBook.AppointmentSystem.Core.Interfaces
{
	public interface IAppointmentService
	{
		Task<string> BookAppointmentAsync(Appointment appointment);
		Task<bool> UpdateAppointmentAsync(UpdateAppointmentDto appointment);
		Task<bool> CancelAppointmentAsync(int appointmentId);
		Task<IEnumerable<AppointmentDto>> GetAppointmentsByPatientId(int patientId);
	}
}
