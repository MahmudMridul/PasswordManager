using PasswordManager.Api.Models;

namespace PasswordManager.Api.Services
{
    public interface IGoogleAuthService
    {
        Task<GoogleUserInfo?> ValidateGoogleTokenAsync(string idToken);
    }

    public class GoogleUserInfo
    {
        public string GoogleId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; }
    }
}