using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebApplication2.DataBaseContext;
using WebApplication2.Model;
using WebApplication2.Request;
using WebApplication2.Interfaces;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        private readonly IBooksService _booksService;
        public BooksController(IBooksService booksService)
        {
            _booksService = booksService;
        }

        public class GetAllBooks
        {
            public int BooksId { get; set; }
            public string Title { get; set; }
            public string GenreName { get; set; }
            public string Description { get; set; }
            public int YearOfPublication { get; set; }
            public int AvailableCopies { get; set; }
        }

        [HttpGet]
        [Route("getAllBooks")]
        public async Task<IActionResult> GetBooks()
        {
            return await _booksService.GetBooks();
        }

        [HttpPost]
        [Route("CreateNewBooks")]
        public async Task<IActionResult> CreateNewBooks([FromQuery] CreateNewBooks newBooks)
        {
            return await _booksService.CreateNewBooks(newBooks);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            return await _booksService.GetBookById(id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            return await _booksService.DeleteBook(id);
        }

        [HttpGet("byGenre/")]
        public async Task<IActionResult> GetBooksByGenre(string genreName)
        {
            return await _booksService.GetBooksByGenre(genreName);
        }

        [HttpGet("{id}/available-copies")]
        public async Task<IActionResult> GetAvailableCopies(int id)
        {
            return await _booksService.GetAvailableCopies(id);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchBooks(string title = null, string genre = null, int? yearOfPublication = null)
        {
            return await _booksService.SearchBooks(title, genre, yearOfPublication);
        }
    }
}
