using MediBook.AppointmentSystem.Core.Entities;
using MediBook.AppointmentSystem.Core.Interfaces;
using MediBook.AppointmentSystem.Infrastructure.Data;

namespace MediBook.AppointmentSystem.Services
{
	public class DoctorService : IDoctorService
	{
		private readonly AppDbContext _context;
		public DoctorService(AppDbContext context) => _context = context;

		public IEnumerable<Doctor> GetDoctorsBySpecialty(string specialty) =>
			_context.Doctors.Where(d => d.Specialty == specialty).ToList();
	}
}
