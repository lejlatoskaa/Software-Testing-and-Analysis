using System.ComponentModel.DataAnnotations;

namespace DentalAppointment.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        public string PatientName { get; set; }
        public DateTime Date { get; set; }
        public string Dentist { get; set; }
        public string Procedure { get; set; }
    }
}