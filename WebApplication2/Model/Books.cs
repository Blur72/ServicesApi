using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Model
{
    public class Books
    {
        [Key]
        public int BooksId { get; set; }
        public string Title { get; set; }

        [Required]
        [ForeignKey("Author")]

        public int AuthorId { get; set; }
        public Author Author { get; set; }

        [Required]
        [ForeignKey("Genre")]

        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public string Description { get; set; }
        public int YearOfPublication { get; set; }
        public int AvailableCopies { get; set; }
    }
}
