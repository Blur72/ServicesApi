using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DataBaseContext;
using WebApplication2.Interfaces;
using WebApplication2.Model;
using WebApplication2.Request;

namespace WebApplication2.Interfaces
{
    public interface IAuthorService
    {
        Task<IActionResult> GetAutors();
        Task<IActionResult> CreateNewAutors([FromQuery] CreateNewAuthor newAutors);
        Task<IActionResult> UpdateAutors(int id, [FromQuery] UpdateAutors updateAutors);
        Task<IActionResult> DeleteAutors(int id);

    }
}
