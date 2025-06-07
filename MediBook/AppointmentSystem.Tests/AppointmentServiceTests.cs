using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MediBook.AppointmentSystem.Infrastructure.Data;
using MediBook.AppointmentSystem.Core.Entities;
using MediBook.AppointmentSystem.Services;
using MediBook.AppointmentSystem.API.Models;

namespace MediBook.AppointmentSystem.Tests
{
	public class AppointmentServiceTests
	{
        private readonly DbContextOptions<AppDbContext> _options;
        private readonly IMemoryCache _cache;

        public AppointmentServiceTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _cache = new MemoryCache(new MemoryCacheOptions());
        }

        [Fact]
        public async Task BookAppointmentAsync_Should_Allow_Only_One_Concurrent_Booking()
        {
            using var context = new AppDbContext(_options);
            var service = new AppointmentService(context, null, _cache);

            var appointment = new Appointment
            {
                DoctorId = 1,
                PatientId = 10,
                AppointmentDateTime = DateTime.UtcNow.AddHours(1),
                Notes = "Checkup"
            };

            var tasks = new List<Task<string>>();
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Run(() => service.BookAppointmentAsync(appointment)));
            }

            var results = await Task.WhenAll(tasks);
            var successCount = results.Count(r => r == "Appointment booked successfully.");
            var failCount = results.Count(r => r == "Slot already taken.");

            Assert.Equal(1, successCount);
            Assert.Equal(9, failCount);
        }

        [Fact]
        public async Task CancelAppointmentAsync_Should_Cancel_Successfully()
        {
            using var context = new AppDbContext(_options);
            var service = new AppointmentService(context, null, _cache);

            var appt = new Appointment
            {
                DoctorId = 2,
                PatientId = 20,
                AppointmentDateTime = DateTime.UtcNow.AddDays(1),
                Status = Enum.Booked.ToString()
            };

            await context.Appointments.AddAsync(appt);
            await context.SaveChangesAsync();

            var result = await service.CancelAppointmentAsync(appt.AppointmentId);
            Assert.True(result);

            var cancelled = await context.Appointments.FindAsync(appt.AppointmentId);
            Assert.Equal(Enum.Cancelled.ToString(), cancelled.Status);
        }

        [Fact]
        public async Task UpdateAppointmentAsync_Should_Update_And_ClearCache()
        {
            using var context = new AppDbContext(_options);
            var service = new AppointmentService(context, null, _cache);

            var appt = new Appointment
            {
                DoctorId = 3,
                PatientId = 30,
                AppointmentDateTime = DateTime.UtcNow,
                Status = Enum.Booked.ToString(),
                Notes = "Old"
            };

            await context.Appointments.AddAsync(appt);
            await context.SaveChangesAsync();

            _cache.Set($"Appointments_{appt.PatientId}", new List<AppointmentDto>());

            var dto = new UpdateAppointmentDto
            {
                AppointmentId = appt.AppointmentId,
                DoctorId = 99,
                AppointmentDateTime = DateTime.UtcNow.AddHours(2),
                Notes = "Updated"
            };

            var result = await service.UpdateAppointmentAsync(dto);
            Assert.True(result);
            Assert.False(_cache.TryGetValue($"Appointments_{appt.PatientId}", out _));
        }

        [Fact]
        public async Task GetAppointmentsByPatientId_Should_Return_From_Cache_If_Exists()
        {
            var patientId = 40;
            var expectedList = new List<AppointmentDto> {
            new AppointmentDto {
                AppointmentId = 1,
                DoctorId = 1,
                DoctorName = "Dr. A",
                AppointmentDateTime = DateTime.UtcNow,
                Status = "Booked",
                PatientName = "P",
                PatientEmail = "p@email.com"
            }
        };

            _cache.Set($"Appointments_{patientId}", expectedList);

            using var context = new AppDbContext(_options);
            var service = new AppointmentService(context, null, _cache);
            var result = await service.GetAppointmentsByPatientId(patientId);

            Assert.Equal(expectedList.Count, result.Count());
            Assert.Equal(expectedList.First().DoctorId, result.First().DoctorId);
        }
    }
}