using Microsoft.AspNetCore.Mvc;
using WebApplication2.Request;

namespace WebApplication2.Interfaces
{
    public interface IBooksService
    {
        Task<IActionResult> GetBooks();
        Task<IActionResult> CreateNewBooks([FromQuery] CreateNewBooks newBooks);
        Task<IActionResult> GetBookById(int id);
        Task<IActionResult> DeleteBook(int id);
        Task<IActionResult> GetBooksByGenre(string genreName);
        Task<IActionResult> GetAvailableCopies(int id);
        Task<IActionResult> SearchBooksByTitle(string title);
        Task<IActionResult> SearchBooks(string title, string genre, int? yearOfPublication);
    }
}
