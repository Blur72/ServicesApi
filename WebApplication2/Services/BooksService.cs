using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                AuthorId = newBooks.AuthorId,
                GenreId = newBooks.GenreId,
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

        public async Task<IActionResult> GetBooks(int page, int pageSize)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return new BadRequestObjectResult("Параметры страницы и размера страницы должны быть положительными.");
            }
            try
            {
                var totalBooks = await _context.Books.CountAsync();
                var totalPages = (int)Math.Ceiling(totalBooks / (double)pageSize);

                var books = await _context.Books
                    .Include(b => b.Author)
                    .Include(b => b.Genre)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                var booksDto = books.Select(b => new GetAllBooks
                {
                    BooksId = b.BooksId,
                    Title = b.Title,
                    AuthorId = b.Author.AuthorId,
                    GenreId = b.Genre.GenreId,
                    AvailableCopies = b.AvailableCopies,
                    YearOfPublication = b.YearOfPublication,
                    Description = b.Description
                });
                var result = new
                {
                    CurrentPage = page,
                    TotalPages = totalPages,
                    TotalBooks = totalBooks,
                    Books = booksDto
                };
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }


        public async Task<IActionResult> SearchBooksByTitle([FromQuery] string title)
        {
            var books = await _context.Books.Where(b => b.Title.Contains(title)).ToListAsync();
            var booksDto = books.Select(b => new GetAllBooks
            {
                BooksId = b.BooksId,
                Title = b.Title,
                AuthorId = b.Author.AuthorId,
                GenreId = b.Genre.GenreId,
                Description = b.Description,
                AvailableCopies = b.AvailableCopies,
                YearOfPublication= b.YearOfPublication
            });
            return new OkObjectResult(booksDto);
        }


        public async Task<IActionResult> GetBooksByGenre(int genreId)
        {
            if (genreId <= 0)
            {
                return new BadRequestObjectResult("Некорректные данные для обновления автора.");
            }
            try
            {
                var books = await _context.Books
                    .Where(b => b.GenreId == genreId)
                    .ToListAsync();
                return new OkObjectResult(books);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> SearchBooks(string title = null, string author = null, string genre = null, int? yearOfPublication = null)
        {
            int criteriaCount = 0;
            if (!string.IsNullOrEmpty(title)) criteriaCount++;
            if (!string.IsNullOrEmpty(author)) criteriaCount++;
            if (!string.IsNullOrEmpty(genre)) criteriaCount++;
            if (yearOfPublication.HasValue) criteriaCount++;

            if (criteriaCount != 1)
            {
                return new BadRequestObjectResult("Необходимо указать хотя бы один критерий поиска.");
            }

            try
            {
                var query = _context.Books.Include(b => b.Author).Include(b => b.Genre).AsQueryable();

                if (!string.IsNullOrEmpty(title))
                {
                    query = query.Where(b => b.Title.Contains(title));
                }

                if (!string.IsNullOrEmpty(author))
                {
                    query = query.Where(b => b.Author.FName.Contains(author) || b.Author.LName.Contains(author));
                }

                if (!string.IsNullOrEmpty(genre))
                {
                    query = query.Where(b => b.Genre.GenreName.Contains(genre));
                }

                if (yearOfPublication.HasValue)
                {
                    query = query.Where(b => b.YearOfPublication == yearOfPublication.Value);
                }

                var books = await query.ToListAsync();

                if (books == null || !books.Any())
                {
                    return new NotFoundObjectResult("Книги с указанным запросом не найдены.");
                }

                var booksDto = books.Select(b => new GetAllBooks
                {
                    BooksId = b.BooksId,
                    Title = b.Title,
                    AuthorId = b.Author.AuthorId,
                    GenreId = b.Genre.GenreId,
                    Description = b.Description,
                    AvailableCopies = b.AvailableCopies,
                    YearOfPublication = b.YearOfPublication
                });

                return new OkObjectResult(booksDto);
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> UpdateBook(int id, UpdateBooks updateBooks)
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

                book.Title = updateBooks.Title;
                book.YearOfPublication = updateBooks.YearOfPublication;
                book.Description = updateBooks.Description;
                book.AvailableCopies = updateBooks.AvailableCopies;
                book.GenreId = updateBooks.GenreId;
                book.AuthorId = updateBooks.AuthorId;

                _context.Books.Update(book);
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
