using Microsoft.AspNetCore.Mvc;
using WebApplication2.Request;

namespace WebApplication2.Interfaces
{
    public interface IRentalService
    {
        Task<IActionResult> RentBook([FromQuery] RentBook request);
        Task<IActionResult> ReturnBook([FromQuery] ReturnBookRequest request);
        Task<IActionResult> GetRentalHistoryByUser(int BooksId);
        Task<IActionResult> GetCurrentRentals();
        Task<IActionResult> GetRentalHistoryByBook(int bookId);
    }
}
