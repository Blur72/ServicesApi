using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DataBaseContext;
using WebApplication2.Interfaces;
using WebApplication2.Model;
using WebApplication2.Request;
using static WebApplication2.Controllers.GenreController;

namespace WebApplication2.Services
{
    public class GenreService : IGenreService
    {
        private readonly TestApiDB2 _context;
        public GenreService(TestApiDB2 context)
        {
            _context = context;
        }
        public async Task<IActionResult> CreateNewGenre([FromQuery] CreateNewGenre newGenre)
        {
            try
            {
                var genre = new Genre()
                {
                    GenreName = newGenre.GenreName
                };
                await _context.Genre.AddAsync(genre);
                await _context.SaveChangesAsync();
                return new OkResult();
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> DeleteGenre(int id)
        {
            if (id <= 0)
            {
                return new BadRequestObjectResult("Некорректные данные для обновления автора.");
            }
            try
            {
                var genres = await _context.Genre.FindAsync(id);
                if (genres == null)
                {
                    return new NotFoundObjectResult("Автор с указанным идентификатором не найден.");
                }

                _context.Genre.Remove(genres);
                await _context.SaveChangesAsync();

                return new NoContentResult();
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> GetGenres()
        {
            try
            {
                var genres = await _context.Genre.ToListAsync();
                var genreDto = genres.Select(b => new GetAllGenres
                {
                    GenreName = b.GenreName
                });
                return new OkObjectResult(genreDto);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> UpdateGenre(int id, [FromQuery] UpdateGenre updateGenre)
        {
            if (id <= 0)
            {
                return new BadRequestObjectResult("Некорректные данные для обновления жанра.");
            }
            try
            {
                var genres = await _context.Genre.FindAsync(id);
                if (genres == null)
                {
                    return new NotFoundObjectResult("Автор с указанным идентификатором не найден.");
                }

                genres.GenreName = updateGenre.GenreName;

                _context.Genre.Update(genres);
                await _context.SaveChangesAsync();

                return new NoContentResult();
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }
    }
}
