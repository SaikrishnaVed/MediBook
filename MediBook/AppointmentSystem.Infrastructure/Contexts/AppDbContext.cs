using MediBook.AppointmentSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediBook.AppointmentSystem.Infrastructure.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
		public DbSet<Doctor> Doctors { get; set; }
		public DbSet<Patient> Patients { get; set; }
		public DbSet<Appointment> Appointments { get; set; }
	}
}