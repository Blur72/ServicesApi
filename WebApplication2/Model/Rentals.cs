using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Model
{
    public class Rentals
    {
        [Key]
        public int RentalId { get; set; }
        [Required]
        [ForeignKey("Books")]
        public int BookId { get; set; }
        public Books Book { get; set; }
        [Required]
        [ForeignKey("Readers")]
        public int ReaderId { get; set; }
        public Readers Reader { get; set; }
        public DateTime DueDate { get; set; }

        public DateTime RentDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}