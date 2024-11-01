using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using WebApplication2.DataBaseContext;
using WebApplication2.Interfaces;
using WebApplication2.Model;
using WebApplication2.Request;
using static AutorsController;

namespace WebApplication2.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly TestApiDB2 _context;
        public AuthorService(TestApiDB2 context) 
        {
            _context = context;
        }

        public async Task<IActionResult> CreateNewAutors([FromQuery] CreateNewAuthor newAutors)
        {
            try
            {
                var autors = new Author()
                {
                    FName = newAutors.FName,
                    LName = newAutors.LName,
                };
                await _context.Author.AddAsync(autors);
                await _context.SaveChangesAsync();
                return new OkResult();
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> DeleteAutors(int id)
        {
            if (id <= 0)
            {
                return new BadRequestObjectResult("Некорректные данные для обновления автора.");
            }
            try
            {
                var autors = await _context.Author.FindAsync(id);
                if (autors == null)
                {
                    return new NotFoundObjectResult("Автор с указанным идентификатором не найден.");
                }

                _context.Author.Remove(autors);
                await _context.SaveChangesAsync();

                return new NoContentResult();
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }
        public async Task<IActionResult> GetAutors()
        {
            var autors = await _context.Author.ToListAsync();
            var autorsDto = autors.Select(b => new GetAllAuthors
            {
                AuthorId = b.AuthorId,
                FName = b.FName,
                LName = b.LName
            });
            return new OkObjectResult(autorsDto);
        }

        public async Task<IActionResult> UpdateAutors(int id, [FromQuery] UpdateAutors updateAutors)
        {
            if (id <= 0)
            {
                return new BadRequestObjectResult("Некорректные данные для обновления автора.");
            }
            try
            {
                var autors = await _context.Author.FindAsync(id);
                if (autors == null)
                {
                    return new NotFoundObjectResult("Автор с указанным идентификатором не найден.");
                }

                autors.FName = updateAutors.FName;
                autors.LName = updateAutors.LName;

                _context.Author.Update(autors);
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
