using System.ComponentModel.DataAnnotations;

namespace MediBook.AppointmentSystem.Core.Entities
{
	public class Patient
	{
        [Key]
        public int PatientId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
    }
}
