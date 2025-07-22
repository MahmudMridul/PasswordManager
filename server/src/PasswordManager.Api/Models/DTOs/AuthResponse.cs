namespace PasswordManager.Api.Models.DTOs
{
    public class AuthResponse
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; }
        public string Token { get; set; } = string.Empty;
        public bool IsNewUser { get; set; }
    }
}