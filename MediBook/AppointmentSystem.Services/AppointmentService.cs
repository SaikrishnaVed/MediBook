using AutoMapper;
using MediBook.AppointmentSystem.API.Models;
using MediBook.AppointmentSystem.Core.Entities;
using MediBook.AppointmentSystem.Core.Interfaces;
using MediBook.AppointmentSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace MediBook.AppointmentSystem.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly AppDbContext _context;
        private static readonly object _lock = new object();
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);


        public AppointmentService(AppDbContext context, IMapper mapper, IMemoryCache cache)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<string> BookAppointmentAsync(Appointment appointment)
        {
            await _semaphore.WaitAsync();
            try
            {
                bool isSlotTaken = await _context.Appointments.AnyAsync(a =>
                    a.DoctorId == appointment.DoctorId &&
                    a.AppointmentDateTime == appointment.AppointmentDateTime &&
                    a.Status == Enum.Booked.ToString());

                if (isSlotTaken)
                    return "Slot already taken.";

                appointment.Status = Enum.Booked.ToString();
                await _context.Appointments.AddAsync(appointment);
                await _context.SaveChangesAsync();

                return "Appointment booked successfully.";
            }
            finally
            {
                _semaphore.Release();
            }
        }


        public async Task<bool> CancelAppointmentAsync(int appointmentId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null) return false;
            appointment.Status = Enum.Cancelled.ToString();
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAppointmentAsync(UpdateAppointmentDto updatedAppointment)
        {
            await _semaphore.WaitAsync();
            try
            {
                var appointment = await _context.Appointments.FindAsync(updatedAppointment.AppointmentId);
                if (appointment == null)
                    return false;

                appointment.AppointmentDateTime = updatedAppointment.AppointmentDateTime;
                appointment.DoctorId = updatedAppointment.DoctorId;
                appointment.Status = Enum.Booked.ToString();
                appointment.Notes = updatedAppointment.Notes;

                await _context.SaveChangesAsync();

                // Clear cache
                var cacheKey = $"Appointments_{appointment.PatientId}";
                _cache.Remove(cacheKey);

                return true;
            }
            finally
            {
                _semaphore.Release();
            }
        }



        public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByPatientId(int patientId)
        {
            var cacheKey = $"Appointments_{patientId}";
            if (!_cache.TryGetValue(cacheKey, out List<AppointmentDto> appointments))
            {
                appointments = await _context.Appointments
                                    .Where(a => a.PatientId == patientId)
                                    .Select(a => new AppointmentDto
                                    {
                                        AppointmentId = a.AppointmentId,
                                        AppointmentDateTime = a.AppointmentDateTime,
                                        Status = a.Status,
                                        DoctorName = a.Doctor.Name,
                                        DoctorId = a.Doctor.DoctorId,
                                        PatientName = a.Patient.Name,
                                        PatientEmail = a.Patient.Email
                                    })
                                    .AsNoTracking()
                                    .ToListAsync();

                _cache.Set(cacheKey, appointments, TimeSpan.FromMinutes(5));
            }

            return appointments;
        }
	}
}
