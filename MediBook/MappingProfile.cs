using AutoMapper;
using MediBook.AppointmentSystem.API.Models;
using MediBook.AppointmentSystem.Core.Entities;

namespace MediBook
{
	public class MappingProfile : Profile
	{
        public MappingProfile()
        {
            CreateMap<AppointmentDto, Appointment>();
        }
    }
}
