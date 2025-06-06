using System.ComponentModel.DataAnnotations;

namespace MediBook.AppointmentSystem.Core.Entities
{
	public class Doctor
	{
        [Key]
        public int DoctorId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Specialty { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
    }
}
