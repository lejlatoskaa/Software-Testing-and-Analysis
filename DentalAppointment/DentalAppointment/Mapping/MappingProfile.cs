using AutoMapper;
using DentalAppointment.DTOs;
using DentalAppointment.Models;

namespace DentalAppointment.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppointmentDTO, Appointment>();
            CreateMap< Appointment, AppointmentDTO>();
        }
    }
}
