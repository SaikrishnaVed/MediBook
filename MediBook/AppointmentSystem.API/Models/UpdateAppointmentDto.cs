namespace MediBook.AppointmentSystem.API.Models
{
	public class UpdateAppointmentDto
	{
        public int AppointmentId { get; set; }

        public DateTime AppointmentDateTime { get; set; }

        public string Status { get; set; }

        public int DoctorId { get; set; }

        public int PatientId { get; set; }


        public string Notes { get; set; }
    }
}
