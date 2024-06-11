namespace DentalAppointment.DTOs
{
    public class AppointmentDTO
    {
        public string PatientName { get; set; }
        public DateTime Date { get; set; }
        public string Dentist { get; set; }
        public string Procedure { get; set; }
    }
}
