using DentalAppointment.Data;
using DentalAppointment.DTOs;
using DentalAppointment.Models;
using DentalAppointment.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DentalAppointment.Repositories.Implementations
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly ApplicationDbContext _context;

        public AppointmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAppointment(AppointmentDTO request)
        {
            Appointment requestBody = new Appointment();

            requestBody.Date = request.Date;
            requestBody.PatientName = request.PatientName;
            requestBody.Dentist = request.Dentist;
            requestBody.Procedure = request.Procedure;

            _context.Appointments.Add(requestBody);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAppointmentAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
        {
            return await _context.Appointments.ToListAsync();
        }

        public async Task<Appointment> GetAppointmentByIdAsync(int id)
        {
            return await _context.Appointments.FindAsync(id);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDentistAsync(string dentist)
        {
            return await _context.Appointments
                .Where(a => a.Dentist == dentist)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByPatientNameAsync(string patientName)
        {
            return await _context.Appointments
                .Where(a => a.PatientName == patientName)
                .ToListAsync();
        }

        public async Task UpdateAppointmentAsync(AppointmentDTO request, int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                appointment.PatientName = request.PatientName;
                appointment.Dentist = request.Dentist;
                appointment.Date = request.Date;
                appointment.Procedure = request.Procedure;

                _context.Entry(appointment).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
         
        }
    }
}
