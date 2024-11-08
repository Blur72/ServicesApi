using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;
using WebApplication2.DataBaseContext;
using WebApplication2.Interfaces;
using WebApplication2.Model;
using WebApplication2.Request;
using static WebApplication2.Controllers.BooksController;

namespace WebApplication2.Services
{
    public class BooksService : IBooksService
    {
        private readonly TestApiDB2 _context;
        public BooksService(TestApiDB2 context)
        {
            _context = context;
        }
        public async Task<IActionResult> CreateNewBooks([FromQuery] CreateNewBooks newBooks)
        {
            var books = new Books()
            {
                Title = newBooks.Title,
                GenreName = newBooks.GenreName,
                AvailableCopies = newBooks.AvailableCopies,
                YearOfPublication = newBooks.YearOfPublication,
                Description = newBooks.Description,
            };
            await _context.Books.AddAsync(books);
            await _context.SaveChangesAsync();
            return new OkResult();
        }

        public async Task<IActionResult> DeleteBook(int id)
        {
            if (id <= 0)
            {
                return new BadRequestObjectResult("Некорректные данные для обновления автора.");
            }
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    return new NotFoundResult();
                }

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();

                return new NoContentResult();
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> GetAvailableCopies(int id)
        {
            if (id <= 0)
            {
                return new BadRequestObjectResult("Некорректные данные для обновления автора.");
            }
            try
            {
                var book = await _context.Books.FindAsync(id);

                if (book == null)
                {
                    return new NotFoundObjectResult("Книга с указанным идентификатором не найдена.");
                }

                return new OkObjectResult(book.AvailableCopies);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(book);
        }

        public async Task<IActionResult> GetBooks()
        {
            var books = await _context.Books.ToListAsync();

            var booksDto = books.Select(b => new GetAllBooks
            {
                BooksId = b.BooksId,
                Title = b.Title,
                GenreName = b.GenreName,
                AvailableCopies = b.AvailableCopies,
                YearOfPublication = b.YearOfPublication,
                Description = b.Description
            });
            return new OkObjectResult(booksDto);
        }


        public async Task<IActionResult> SearchBooksByTitle([FromQuery] string title)
        {
            var books = await _context.Books.Where(b => b.Title.Contains(title)).ToListAsync();
            var booksDto = books.Select(b => new GetAllBooks
            {
                BooksId = b.BooksId,
                Title = b.Title,
                GenreName = b.GenreName,
                Description = b.Description,
                AvailableCopies = b.AvailableCopies,
                YearOfPublication= b.YearOfPublication
            });
            return new OkObjectResult(booksDto);
        }


        public async Task<IActionResult> GetBooksByGenre(string genreName)
        {
            if (genreName == null)
            {
                return new BadRequestObjectResult("Некорректные данные для обновления автора.");
            }
            try
            {
                var books = await _context.Books
                    .Where(b => b.GenreName == genreName)
                    .ToListAsync();
                return new OkObjectResult(books);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> SearchBooks(string title = null, string genreName = null, int? yearOfPublication = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(title))
                {
                    var query = _context.Books.Include(b => b.GenreName).AsQueryable();
                    query = query.Where(b => b.Title.Contains(title));
                    var books = await query.ToListAsync();

                    if (books == null || !books.Any())
                    {
                        return new NotFoundObjectResult("Книги с указанным запросом не найдены.");
                    }

                    var booksDto = books.Select(b => new GetAllBooks
                    {
                        BooksId = b.BooksId,
                        Title = b.Title,
                        GenreName = b.GenreName,
                        Description = b.Description,
                        AvailableCopies = b.AvailableCopies,
                        YearOfPublication = b.YearOfPublication
                    });

                    return new OkObjectResult(booksDto);
                }

                if (!string.IsNullOrEmpty(genreName))
                {
                    var query = _context.Books.Include(b => b.GenreName).AsQueryable();
                    query = query.Where(b => b.GenreName.Contains(genreName));
                    var books = await query.ToListAsync();

                    if (books == null || !books.Any())
                    {
                        return new NotFoundObjectResult("Книги с указанным запросом не найдены.");
                    }

                    var booksDto = books.Select(b => new GetAllBooks
                    {
                        BooksId = b.BooksId,
                        Title = b.Title,
                        GenreName = b.GenreName,
                        Description = b.Description,
                        AvailableCopies = b.AvailableCopies,
                        YearOfPublication = b.YearOfPublication
                    });

                    return new OkObjectResult(booksDto);
                }

                if (yearOfPublication.HasValue)
                {
                    var query = _context.Books.Include(b => b.GenreName).AsQueryable();
                    query = query.Where(b => b.YearOfPublication == yearOfPublication.Value);
                    var books = await query.ToListAsync();

                    if (books == null || !books.Any())
                    {
                        return new NotFoundObjectResult("Книги с указанным запросом не найдены.");
                    }

                    var booksDto = books.Select(b => new GetAllBooks
                    {
                        BooksId = b.BooksId,
                        Title = b.Title,
                        GenreName = b.GenreName,
                        Description = b.Description,
                        AvailableCopies = b.AvailableCopies,
                        YearOfPublication = b.YearOfPublication
                    });

                    return new OkObjectResult(booksDto);
                }
                return new OkResult();
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }
    }
}
