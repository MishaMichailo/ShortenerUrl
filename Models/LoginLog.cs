using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace ShortURL.Models
{
    public class LoginLog
    {
        [Key]
        public int Id {  get; set; }       
        public DateTime LoginTime { get; set; }


        [ForeignKey("User")]
        public int UserId { get; set; }  
        public ICollection<User> User { get; set; }
    }
}
