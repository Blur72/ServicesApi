using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Request
{
    public class ReturnBookRequest
    {
        [Required]
        public int RentalId { get; set; }
    }
}
