using System.ComponentModel.DataAnnotations;

namespace MediBook.AppointmentSystem.API.Models
{
    /// <summary>
    /// Represents an appointment data transfer object
    /// </summary>
    public class AppointmentDto
    {
        /// <summary>
        /// Unique identifier for the appointment
        /// </summary>
        public int AppointmentId { get; set; }

        /// <summary>
        /// Date and time of the appointment
        /// </summary>
        [Required]
        public DateTime AppointmentDateTime { get; set; }

        /// <summary>
        /// Current status of the appointment
        /// </summary>
        [Required]
        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters")]
        public string Status { get; set; }

        /// <summary>
        /// Name of the doctor
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "Doctor name cannot exceed 100 characters")]
        public string DoctorName { get; set; }

        /// <summary>
        /// Unique identifier of the doctor
        /// </summary>
        [Required]
        public int DoctorId { get; set; }

        /// <summary>
        /// Name of the patient
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "Patient name cannot exceed 100 characters")]
        public string PatientName { get; set; }

        /// <summary>
        /// Email address of the patient
        /// </summary>
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
        public string PatientEmail { get; set; }

        /// <summary>
        /// Notes of the patient
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "Notes cannot exceed 100 characters")]
        public string Notes { get; set; }
    }
}
