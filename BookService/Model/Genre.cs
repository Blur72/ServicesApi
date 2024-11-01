using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Model
{
    public class Genre
    {
        [Key]
        public int GenreId { get; set; }
        public string GenreName { get; set; }
    }
}
