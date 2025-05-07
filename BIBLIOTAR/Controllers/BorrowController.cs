using AutoMapper;
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
    public class BorrowController : ControllerBase
    {
        private readonly IBorrowService _borrowService;
        private readonly IMapper _mapper;
        public BorrowController(IBorrowService borrowService, IMapper mapper)
        {
            _borrowService = borrowService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("create")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateBorrowCont([FromBody] BorrowCreateDto borrowCreateDto)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var response = await _borrowService.CreateBorrow(borrowCreateDto);
                apiResponse.Data = response;
                apiResponse.Message = "Borrow created successfully";
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
