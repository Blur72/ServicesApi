using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Request;
using WebApplication2.DataBaseContext;
using WebApplication2.Model;
using WebApplication2.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class RentalController : Controller
{
    private readonly IRentalService _rentalService;
    public RentalController(IRentalService rentalService)
    {
        _rentalService = rentalService;
    }

    [HttpPost("rent")]
    public async Task<IActionResult> RentBook([FromQuery] RentBook request)
    {
        return await _rentalService.RentBook(request);
    }

    [HttpPost("return")]
    public async Task<IActionResult> ReturnBook([FromQuery] ReturnBookRequest request)
    {
        return await _rentalService.ReturnBook(request);
    }

    [HttpGet("user/{BooksId}/history")]
    public async Task<IActionResult> GetRentalHistoryByUser(int BooksId)
    {
        return await _rentalService.GetRentalHistoryByUser(BooksId);
    }

    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentRentals()
    {
        return await _rentalService.GetCurrentRentals();
    }

    [HttpGet("book/{bookId}/history")]
    public async Task<IActionResult> GetRentalHistoryByBook(int bookId)
    {
        return await _rentalService.GetRentalHistoryByBook(bookId);
    }
}