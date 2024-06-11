using DentalAppointment.DTOs;
using DentalAppointment.Models;

namespace DentalAppointment.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
        Task<Appointment> GetAppointmentByIdAsync(int id);
        Task<IEnumerable<Appointment>> GetAppointmentsByPatientNameAsync(string patientName);
        Task<IEnumerable<Appointment>> GetAppointmentsByDentistAsync(string dentist);
        Task CreateAppointment(AppointmentDTO resquest);
        Task UpdateAppointmentAsync(AppointmentDTO request, int id);
        Task DeleteAppointmentAsync(int id);
    }
}
