using DentalAppointment.DTOs;
using DentalAppointment.Models;

namespace DentalAppointment.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentDTO>> GetAllAppointmentsAsync();

        Task CreateAppointment(AppointmentDTO request);
        Task UpdateAppointmentAsync(AppointmentDTO request, int id);
        Task DeleteAppointmentAsync(int id);
        Task<Appointment> GetAppointmentByIdAsync(int id);
        Task<IEnumerable<Appointment>> GetAppointmentsByPatientNameAsync(string patientName);
        Task<IEnumerable<Appointment>> GetAppointmentsByDentistAsync(string dentist);
    }
}
