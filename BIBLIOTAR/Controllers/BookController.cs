using BiblioTar.DTOs;
using BiblioTar.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiblioTar.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Register([FromBody] BookCreateDto bookCreateDto)
        {
            var resoult=await _bookService.RegisterBook(bookCreateDto);
            return Ok(resoult);
        }
    }
}
