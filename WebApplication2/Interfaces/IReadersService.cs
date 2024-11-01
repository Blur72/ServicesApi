using Microsoft.AspNetCore.Mvc;
using WebApplication2.Request;

namespace WebApplication2.Interfaces
{
    public interface IReadersService
    {
        Task<IActionResult> GetReaders(int page, int pageSize);
        Task<IActionResult> GetReaderById(int id);
        Task<IActionResult> CreateNewReader([FromQuery] CreateNewReader newReader);
        Task<IActionResult> UpdateReaders(int id, [FromQuery] CreateNewReader updateReaders);
        Task<IActionResult> DeleteAutors(int id);
        Task<IActionResult> SearchReaders(string query);
    }
}
