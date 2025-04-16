using BiblioTar.DTOs;
using BiblioTar.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiblioTar.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("create")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserCreateDto userCreateDto)
        {
            var result = await _userService.RegisterCustomer(userCreateDto);
            return Ok(result);
        }

        [HttpPost]
        [Route("createEmployee")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterEmployee([FromBody] EmployeeCreateDto employeeCreate)
        {
            var result = await _userService.RegisterEmployee(employeeCreate);
            return Ok(result);
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginDto loginDto)
        {
            var user = await _userService.Authenticate(loginDto.Email,loginDto.Password);
            if (user == null) 
            {
                return Unauthorized("Hibás email vagy jelszó.");
            }
            var token=_userService.GenerateToken(user);
            return Ok(new { token });
             
        }




    }
}
