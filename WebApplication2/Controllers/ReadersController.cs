using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WebApplication2.DataBaseContext;
using WebApplication2.Interfaces;
using WebApplication2.Model;
using WebApplication2.Request;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReadersController : Controller
    {
        private readonly IReadersService _readersService;
        public ReadersController(IReadersService readersService)
        {
            _readersService = readersService;
        }

        public class GetAllReaders
        {
            public int ReaderId { get; set; }
            public string FName { get; set; }
            public string LName { get; set; }
            public DateTime DateOfBirth { get; set; }
        }
        [HttpGet]
        [Route("getAllReaders")]
        public async Task<IActionResult> GetReaders(int page, int pageSize)
        {
            return await _readersService.GetReaders(page, pageSize);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReaderById(int id)
        {
            return await _readersService.GetReaderById(id);
        }

        [HttpPost]
        [Route("CreateNewReader")]
        public async Task<IActionResult> CreateNewReader([FromQuery] CreateNewReader newReader)
        {
            return await _readersService.CreateNewReader(newReader);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReaders(int id, [FromQuery] CreateNewReader updateReaders)
        {
            return await _readersService.UpdateReaders(id, updateReaders);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAutors(int id)
        {
            return await _readersService.DeleteAutors(id);
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchBooks(string query)
        {
            return await _readersService.SearchReaders(query);
        }
    }
}
