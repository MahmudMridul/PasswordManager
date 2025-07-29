using Microsoft.EntityFrameworkCore;
using PasswordManager.Api.Models;

namespace PasswordManager.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Password> PasswordEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users", "pm");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.GoogleId).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.GoogleId).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Name).IsRequired();
                
                // PostgreSQL specific: Use timestamp with time zone for DateTime properties
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp with time zone");
                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<Password>(entity =>
            {
                entity.ToTable("Passwords", "pm");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Passwords)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                
                // PostgreSQL specific: Use timestamp with time zone for DateTime properties
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp with time zone");
                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp with time zone");
                entity.Property(e => e.LastUsed)
                    .HasColumnType("timestamp with time zone");
            });
        }
    }
}