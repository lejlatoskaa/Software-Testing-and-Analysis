using Microsoft.AspNetCore.Mvc;
using DentalAppointment.Services;
using DentalAppointment.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using DentalAppointment.Models;
using DentalAppointment.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.Extensions.Caching.Memory;

[Route("api/[controller]")]
[ApiController]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;
    private readonly IMemoryCache _memoryCache;

    public AppointmentController(IAppointmentService appointmentService, IMemoryCache memoryCache)
    {
        _appointmentService = appointmentService;
        _memoryCache = memoryCache;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateAppointment(AppointmentDTO request)
    {
        await _appointmentService.CreateAppointment(request);
        return Ok();
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetAppointment(int id)
    {
        if (!_memoryCache.TryGetValue("myData", out AppointmentDTO appointmentDto))
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            appointmentDto = new AppointmentDTO
            {
                PatientName = appointment.PatientName,
                Date = appointment.Date,
                Dentist = appointment.Dentist,
                Procedure = appointment.Procedure
            };

            _memoryCache.Set("myData", appointmentDto, TimeSpan.FromMinutes(10));
        }

        return Ok(appointmentDto);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllAppointments()
    {
        if (!_memoryCache.TryGetValue("myData", out List<AppointmentDTO> appointmentDtos))
        {
            var appointments = await _appointmentService.GetAllAppointmentsAsync();
            appointmentDtos = appointments.Select(a => new AppointmentDTO
            {
                PatientName = a.PatientName,
                Date = a.Date,
                Dentist = a.Dentist,
                Procedure = a.Procedure
            }).ToList();

            _memoryCache.Set("myData", appointmentDtos, TimeSpan.FromMinutes(10));
        }

        return Ok(appointmentDtos);
    }

    [HttpGet("/byPatients/{patientName}")]
    [Authorize]
    public async Task<IActionResult> GetAppointmentsByPatientNameAsync(string patientName)
    {
        if (!_memoryCache.TryGetValue("myData", out List<Appointment> appointments))
        {
            appointments = (await _appointmentService.GetAppointmentsByPatientNameAsync(patientName)).ToList();
            _memoryCache.Set("myData", appointments, TimeSpan.FromMinutes(10));
        }

        return Ok(appointments);
    }

    [HttpGet("/byDentist/{dentist}")]
    [Authorize]
    public async Task<IActionResult> GetAppointmentsByDentistAsync(string dentist)
    {
        if (!_memoryCache.TryGetValue("myData", out List<Appointment> appointments))
        {
            appointments = (await _appointmentService.GetAppointmentsByDentistAsync(dentist)).ToList();
            _memoryCache.Set("myData", appointments, TimeSpan.FromMinutes(10));
        }

        return Ok(appointments);
    }


    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateAppointment(AppointmentDTO request, int id)
    {
        await _appointmentService.UpdateAppointmentAsync(request, id);
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteAppointment(int id)
    {
        await _appointmentService.DeleteAppointmentAsync(id);
        return Ok();
    }
}
