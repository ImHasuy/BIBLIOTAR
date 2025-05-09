using BiblioTar.DTOs;
using BiblioTar.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiblioTar.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
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

        [HttpDelete]
        [Route("remove")]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var resoult= await _bookService.DeleteBook(id);
            if (resoult)
            {
               return NoContent();
            }
            return NotFound();
        }
        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAllBooks()
        {
            var resoult = await _bookService.GetAllBooks();
            if (resoult == null)
            {
                return NotFound();
            }
            return Ok(resoult);
        }
        [HttpGet]
        [Route("{title}")]
        public async Task<IActionResult> GetBook(string title)
        {
            var resoult = await _bookService.GetBook(title);
            if (resoult == null)
            {
                return NotFound();
            }
            return Ok(resoult);
        }
    }
}
