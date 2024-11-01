using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Request
{
    public class RentBook
    {
        [Required]
        public int BookId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime DueDate { get; set; }
    }
}
