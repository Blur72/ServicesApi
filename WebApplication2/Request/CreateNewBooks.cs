using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebApplication2.Model;

namespace WebApplication2.Request
{
    public class CreateNewBooks
    {
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public int GenreId { get; set; }
        public string Description { get; set; }
        public int YearOfPublication { get; set; }
        public int AvailableCopies { get; set; }
    }
}
