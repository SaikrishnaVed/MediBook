namespace MediBook.AppointmentSystem.API.Models
{
	public class AppointmentDto
	{
		public int DoctorId { get; set; }
		public int PatientId { get; set; }
		public DateTime AppointmentDateTime { get; set; }
		public Enum Status { get; set; }
	}
}
