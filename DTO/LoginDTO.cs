using System.ComponentModel.DataAnnotations;

namespace ShortURL.DTO
{
    public class LoginDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
