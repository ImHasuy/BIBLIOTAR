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





    }
}
