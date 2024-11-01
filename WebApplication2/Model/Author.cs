using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Model
{
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }
        public string LName { get; set; }
        public string FName { get; set; }
    }
}
