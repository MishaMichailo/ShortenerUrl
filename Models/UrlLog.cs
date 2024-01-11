using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShortURL.Models
{
    public class UrlLog
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }

        public string UrlBase { get; set; }

        public string UrlShort { get; set; }

        public ICollection<User> Users { get; set; }

    }
}
