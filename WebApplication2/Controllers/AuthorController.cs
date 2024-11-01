using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DataBaseContext;
using WebApplication2.Interfaces;
using WebApplication2.Model;
using WebApplication2.Request;

[ApiController]
[Route("api/[controller]")]
public class AutorsController : Controller
{
    private readonly IAuthorService _authorService;
    public AutorsController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    public class GetAllAuthors
    {
        public int AuthorId { get; set; }
        public string LName { get; set; }
        public string FName { get; set; }
    }

    [HttpGet]
    [Route("getAllAutors")]
    public async Task<IActionResult> GetAutors()
    {
        return await _authorService.GetAutors();
    }

    [HttpPost]
    [Route("CreateNewAutors")]
    public async Task<IActionResult> CreateNewAutors([FromQuery] CreateNewAuthor newAutors)
    {
        return await _authorService.CreateNewAutors(newAutors);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAutors(int id, [FromQuery] UpdateAutors updateAutors)
    {
        return await _authorService.UpdateAutors(id, updateAutors);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAutors(int id)
    {
        return await _authorService.DeleteAutors(id);
    }
}