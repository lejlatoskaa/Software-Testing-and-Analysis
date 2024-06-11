using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DentalAppointment.DTOs;
using DentalAppointment.Models;
using DentalAppointment.Repositories.Interfaces;
using DentalAppointment.Services.Interfaces;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IMapper _mapper;


    public AppointmentService(IAppointmentRepository appointmentRepository, IMapper mapper)
    {
        _appointmentRepository = appointmentRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AppointmentDTO>> GetAllAppointmentsAsync()
    {
        var appointment = await _appointmentRepository.GetAllAppointmentsAsync();
        var response = appointment?.Select(element =>
        {
            AppointmentDTO appointmentDto = new AppointmentDTO();

            return _mapper.Map(element, appointmentDto);
        });

        return response;
    }

    public async Task<Appointment> GetAppointmentByIdAsync(int id)
    {
        return await _appointmentRepository.GetAppointmentByIdAsync(id);
    }

    public async Task<IEnumerable<Appointment>> GetAppointmentsByPatientNameAsync(string patientName)
    {
        return await _appointmentRepository.GetAppointmentsByPatientNameAsync(patientName);
    }

    public async Task<IEnumerable<Appointment>> GetAppointmentsByDentistAsync(string dentist)
    {
        return await _appointmentRepository.GetAppointmentsByDentistAsync(dentist);
    }

    public async Task CreateAppointment(AppointmentDTO request)
    {

        await _appointmentRepository.CreateAppointment(request);
       
    }


    public async Task UpdateAppointmentAsync(AppointmentDTO request, int id)
    {
        await _appointmentRepository.UpdateAppointmentAsync(request, id);
    }

    public async Task DeleteAppointmentAsync(int id)
    {
        await _appointmentRepository.DeleteAppointmentAsync(id);
    }

    
}
