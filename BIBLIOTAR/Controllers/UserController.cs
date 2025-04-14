using BiblioTar.DTOs;
using BiblioTar.Service;
using Microsoft.AspNetCore.Mvc;

namespace BiblioTar.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] UserCreateDto userCreateDto)
        {
            var result = await _userService.CreateAsync(userCreateDto);
            return Ok(result);
        }
    }
}
