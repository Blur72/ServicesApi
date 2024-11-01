using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DataBaseContext;
using WebApplication2.Interfaces;
using WebApplication2.Model;
using WebApplication2.Request;
using static WebApplication2.Controllers.ReadersController;

namespace WebApplication2.Services
{

    public class ReadersService : IReadersService
    {
        private readonly TestApiDB2 _context;
        public ReadersService(TestApiDB2 context)
        {
            _context = context;
        }
        public async Task<IActionResult> CreateNewReader([FromQuery] CreateNewReader newReader)
        {
            try
            {
                var reader = new Readers()
                {
                    FName = newReader.FName,
                    LName = newReader.LName,
                    DateOfBirth = newReader.DateOfBirth
                };
                await _context.Readers.AddAsync(reader);
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
                return new BadRequestObjectResult("Некорректные данные для обновления читателя.");
            }
            try
            {
                var readers = await _context.Readers.FindAsync(id);
                if (readers == null)
                {
                    return new NotFoundObjectResult("Читатель с указанным идентификатором не найдена.");
                }

                _context.Readers.Remove(readers);
                await _context.SaveChangesAsync();

                return new NoContentResult();
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> GetReaderById(int id)
        {
            if (id <= 0)
            {
                return new BadRequestObjectResult("Некорректные данные для обновления автора.");
            }
            try
            {
                var reader = await _context.Readers.FindAsync(id);
                if (reader == null)
                {
                    return new NotFoundResult();
                }
                return new OkObjectResult(reader);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> GetReaders(int page, int pageSize)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return new BadRequestObjectResult("Параметры страницы и размера страницы должны быть положительными.");
            }

            try
            {
                var totalReaders = await _context.Readers.CountAsync();
                var totalPages = (int)Math.Ceiling(totalReaders / (double)pageSize);

                var readers = await _context.Readers
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var readersDto = readers.Select(r => new GetAllReaders
                {
                    ReaderId = r.ReaderId,
                    FName = r.FName,
                    LName = r.LName,
                    DateOfBirth = r.DateOfBirth
                });

                var result = new
                {
                    CurrentPage = page,
                    TotalPages = totalPages,
                    TotalReaders = totalReaders,
                    Readers = readersDto
                };

                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> SearchReaders(string query)
        {
            if (query == null)
            {
                return new BadRequestObjectResult("Строка обязательна для поиска.");
            }

            try
            {
                var readers = await _context.Readers
                   .Where(b => b.FName.Contains(query) || b.LName.Contains(query))
                   .ToListAsync();

                if (readers == null)
                {
                    return new NotFoundObjectResult("Читатель с указанным идентификатором не найден.");
                }

                var readersDto = readers.Select(b => new GetAllReaders
                {
                    ReaderId = b.ReaderId,
                    FName = b.FName,
                    LName = b.LName,
                    DateOfBirth = b.DateOfBirth
                });

                return new OkObjectResult(readersDto);
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> UpdateReaders(int id, [FromQuery] CreateNewReader updateReaders)
        {
            if (id <= 0)
            {
                return new BadRequestObjectResult("Некорректные данные для обновления читателя.");
            }
            try
            {
                var readers = await _context.Readers.FindAsync(id);
                if (readers == null)
                {
                    return new NotFoundObjectResult("Читатель с указанным идентификатором не найдена.");
                }

                readers.FName = updateReaders.FName;
                readers.LName = updateReaders.LName;

                _context.Readers.Update(readers);
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
