using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediBook.AppointmentSystem.Core.Entities
{
	public class Appointment
	{
        [Key]
        public int AppointmentId { get; set; }

        [Required]
        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }

        [Required]
        [ForeignKey("Patient")]
        public int PatientId { get; set; }

        [Required]
        public DateTime AppointmentDateTime { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; }

        public string Notes { get; set; }

        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
    }
}
