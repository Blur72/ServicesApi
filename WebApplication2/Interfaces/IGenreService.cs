using Microsoft.AspNetCore.Mvc;
using WebApplication2.Request;

namespace WebApplication2.Interfaces
{
    public interface IGenreService
    {
        Task<IActionResult> GetGenres();
        Task<IActionResult> CreateNewGenre([FromQuery] CreateNewGenre newGenre);
        Task<IActionResult> UpdateGenre(int id, [FromQuery] UpdateGenre updateGenre);
        Task<IActionResult> DeleteGenre(int id);
    }
}
