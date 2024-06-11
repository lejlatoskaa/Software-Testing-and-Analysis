namespace DentalAppointment.DTOs
{
    public class AuthenticationResponseDTO
    {
        public string UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
