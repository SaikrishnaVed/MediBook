using MediBook.AppointmentSystem.Core.Entities;

namespace MediBook.AppointmentSystem.Core.Interfaces
{
	public interface IDoctorService
	{
		IEnumerable<Doctor> GetDoctorsBySpecialty(string specialty);
	}
}
