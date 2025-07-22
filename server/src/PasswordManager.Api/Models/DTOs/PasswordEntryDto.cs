namespace PasswordManager.Api.Models.DTOs
{
    public class PasswordEntryDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Website { get; set; }
        public string? Notes { get; set; }
        public bool IsFavorite { get; set; }
    }
}