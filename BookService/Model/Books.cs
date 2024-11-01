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
        public string GenreName { get; set; }
        public string Description { get; set; }
        public int YearOfPublication { get; set; }
        public int AvailableCopies { get; set; }
    }
}
