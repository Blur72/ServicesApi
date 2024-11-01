using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DataBaseContext;
using WebApplication2.Interfaces;
using WebApplication2.Model;
using WebApplication2.Request;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenreController : Controller
    {
        private readonly IGenreService _genreService;
        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        public class GetAllGenres
        {
            public int GenreId { get; set; }
            public string GenreName { get; set; }
        }

        [HttpGet]
        [Route("getAllGenre")]
        public async Task<IActionResult> GetGenres()
        {
            return await _genreService.GetGenres();
        }

        [HttpPost]
        [Route("CreateNewGenre")]
        public async Task<IActionResult> CreateNewGenre([FromQuery] CreateNewGenre newGenre)
        {
            return await _genreService.CreateNewGenre(newGenre);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGenre(int id, [FromQuery] UpdateGenre updateGenre)
        {
            return await _genreService.UpdateGenre(id, updateGenre);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            return await _genreService.DeleteGenre(id);
        }
    }
}
