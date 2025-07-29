namespace PasswordManager.Api.Models
{
    public class Password
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string EncryptedPassword { get; set; } = string.Empty;
        public string? Website { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? LastUsed { get; set; }
        public bool IsFavorite { get; set; } = false;
        
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}