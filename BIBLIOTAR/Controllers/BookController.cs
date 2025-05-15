using BiblioTar.DTOs;
using BiblioTar.Entities;
using BiblioTar.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiblioTar.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost]
        [Route("create")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Register([FromBody] BookCreateDto bookCreateDto)
        {
            var resoult=await _bookService.RegisterBook(bookCreateDto);
            return Ok(resoult);
        }

        [HttpDelete]
        [Route("remove/{bookId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Delete(int bookId)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var temp = await _bookService.DeleteBook(bookId);
                apiResponse.Message = $"The outcome of the delete operation: {temp}";
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                apiResponse.StatusCode = 400;
                apiResponse.Message = ex.Message;
                apiResponse.Success = false;
            }
            return BadRequest(apiResponse); 
        }
        [HttpGet]
        [Route("getall")]
        [AllowAnonymous]
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
        [AllowAnonymous]
        public async Task<IActionResult> GetBook(string title)
        {
            var resoult = await _bookService.GetBook(title);
            if (resoult == null)
            {
                return NotFound();
            }
            return Ok(resoult);
        }
        
        
        [HttpPut]
        [Route("UpdateBook")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateBook(BookUpdateDto bookUpdateDto)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var temp =await _bookService.UpdateBook(bookUpdateDto);
                apiResponse.Message = temp;
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                apiResponse.StatusCode = 400;
                apiResponse.Message = ex.Message;
                apiResponse.Success = false;
            }
            return BadRequest(apiResponse); 
        }
    }
}
