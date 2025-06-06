using MediBook.AppointmentSystem.Core.Entities;

namespace MediBook.AppointmentSystem.Core.Interfaces
{
	public interface IAppointmentService
	{
		Task<string> BookAppointmentAsync(Appointment appointment);
		Task<bool> CancelAppointmentAsync(int appointmentId);
		Task<IEnumerable<Appointment>> GetAppointmentsByPatientId(int patientId);
	}
}
