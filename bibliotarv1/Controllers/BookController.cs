using bibliotar.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bibliotar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBibliotarService _bookService;

        public BookController(IBibliotarService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public IActionResult List()
        {
            var result = _bookService.List();
            return Ok(result);
        }


    }
}
