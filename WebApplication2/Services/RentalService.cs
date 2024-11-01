using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DataBaseContext;
using WebApplication2.Interfaces;
using WebApplication2.Model;
using WebApplication2.Request;

namespace WebApplication2.Services
{
    public class RentalService : IRentalService
    {
        private readonly TestApiDB2 _context;
        public RentalService(TestApiDB2 context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetCurrentRentals()
        {
            try
            {
                var rentals = await _context.Rentals
                    .Where(r => r.ReturnDate == null)
                    .Include(r => r.Book)
                    .Include(r => r.Reader)
                    .Select(r => new
                    {
                        BookTitle = r.Book.Title,
                        UserName = r.Reader.FName + " " + r.Reader.LName,
                        RentalDate = r.RentDate,
                        DueDate = r.DueDate
                    })
                    .ToListAsync();

                return new OkObjectResult(rentals);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> GetRentalHistoryByBook(int bookId)
        {
            if (bookId <= 0)
            {
                return new BadRequestObjectResult("Некорректные данные для обновления автора.");
            }
            try
            {
                var rentals = await _context.Rentals
                    .Where(r => r.BookId == bookId)
                    .Include(r => r.Book)
                    .Include(r => r.Reader)
                    .Select(r => new
                    {
                        BookTitle = r.Book.Title,
                        UserName = r.Reader.FName + " " + r.Reader.LName,
                        RentalDate = r.RentDate,
                        DueDate = r.DueDate,
                        ReturnDate = r.ReturnDate
                    })
                    .ToListAsync();

                if (rentals == null || rentals.Count == 0)
                {
                    return new NotFoundObjectResult("История аренды для книги не найдена.");
                }

                return new OkObjectResult(rentals);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> GetRentalHistoryByUser(int BooksId)
        {
            if (BooksId <= 0)
            {
                return new BadRequestObjectResult("Некорректные данные для обновления автора.");
            }
            try
            {
                var rentals = await _context.Rentals
                    .Where(r => r.BookId == BooksId)
                    .Include(r => r.Book)
                    .Include(r => r.Reader)
                    .Select(r => new
                    {
                        BookTitle = r.Book.Title,
                        UserName = r.Reader.FName + " " + r.Reader.LName,
                        RentalDate = r.RentDate,
                        DueDate = r.DueDate,
                        ReturnDate = r.ReturnDate
                    })
                    .ToListAsync();

                if (rentals == null || rentals.Count == 0)
                {
                    return new NotFoundObjectResult("История аренды не найдена.");
                }

                return new OkObjectResult(rentals);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> RentBook([FromQuery] RentBook request)
        {
            try
            {
                var book = await _context.Books.FindAsync(request.BookId);
                if (book == null)
                {
                    return new NotFoundObjectResult("Книга не найдена.");
                }

                if (book.AvailableCopies <= 0)
                {
                    return new BadRequestObjectResult("Нет доступных копий для аренды.");
                }
                var reader = await _context.Readers.FirstOrDefaultAsync(g => g.ReaderId == request.UserId);
                var rental = new Rentals()
                {
                    BookId = request.BookId,
                    ReaderId = reader.ReaderId,
                    RentDate = request.DueDate
                };

                book.AvailableCopies--;

                _context.Rentals.Add(rental);
                await _context.SaveChangesAsync();

                return new OkObjectResult("Книга арендована.");
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> ReturnBook([FromQuery] ReturnBookRequest request)
        {
            try
            {
                var rental = await _context.Rentals.FindAsync(request.RentalId);
                if (rental == null)
                {
                    return new NotFoundObjectResult("Аренда не найдена.");
                }

                var book = await _context.Books.FindAsync(rental.BookId);
                if (book == null)
                {
                    return new NotFoundObjectResult("Книга не найдена.");
                }

                rental.ReturnDate = DateTime.UtcNow;
                book.AvailableCopies++;

                await _context.SaveChangesAsync();

                return new OkObjectResult("Книга возвращена.");
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }
    }
}
