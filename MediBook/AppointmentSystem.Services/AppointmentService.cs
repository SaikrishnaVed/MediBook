using AutoMapper;
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

        public AppointmentService(AppDbContext context, IMapper mapper, IMemoryCache cache)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<string> BookAppointmentAsync(Appointment appointment)
        {
            lock (_lock)
            {
                bool isSlotTaken = _context.Appointments.Any(a =>
                    a.DoctorId == appointment.DoctorId &&
                    a.AppointmentDateTime == appointment.AppointmentDateTime &&
                    a.Status == Enum.Booked.ToString());

                if (isSlotTaken) return "Slot already taken.";

                appointment.Status = Enum.Booked.ToString();
                _context.Appointments.Add(appointment);
                _context.SaveChanges();
                return "Appointment booked successfully.";
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

        public async Task<IEnumerable<Appointment>> GetAppointmentsByPatientId(int patientId)
        {
            var cacheKey = $"Appointments_{patientId}";
            if (!_cache.TryGetValue(cacheKey, out List<Appointment> appointments))
            {
                appointments = await _context.Appointments
                    //.Include(a => a.Doctor)
                    .Where(a => a.PatientId == patientId)
                    .ToListAsync();

                _cache.Set(cacheKey, appointments, TimeSpan.FromMinutes(5));
            }

            return appointments;
        }
    }
}
