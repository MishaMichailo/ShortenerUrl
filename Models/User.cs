using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace ShortURL.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }


      
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; } = DateTime.Now;
        public DateTime TokenExpires { get; set; } = DateTime.Now;
        public string? Role { get; set; } = "user";
        public RegLog RegLogs { get; set; }
        public ICollection<UrlLog> UrlLogs { get; set; }
        public ICollection<LoginLog> LoginLogs { get; set; }

        
    }
}
