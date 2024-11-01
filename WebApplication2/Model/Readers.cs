using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Model
{
    public class Readers
    {
        [Key]
        public int ReaderId { get; set; }
        [Required]
        public string FName { get; set; }
        public string LName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
