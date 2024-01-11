using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShortURL.Models
{
    public class RegLog
    {
        [Key]
        public int Id { get; set; }
        public DateTime RegisterTime { get; set; }

        //
        [ForeignKey("User")]
        public int UserId {  get; set; }
        public User User { get; set; }
    }

}
