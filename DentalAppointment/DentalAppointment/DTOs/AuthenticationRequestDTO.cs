namespace DentalAppointment.DTOs
{
    public class AuthenticationRequestDTO
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
