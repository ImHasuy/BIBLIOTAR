using System.Security.Claims;
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
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var result = await _userService.RegisterCustomer(userCreateDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                apiResponse.StatusCode = 400;
                apiResponse.Message = ex.Message;
                apiResponse.Success = false;
            }
            return BadRequest(apiResponse); 
            
        }

        [HttpPost]
        [Route("createEmployee")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterEmployee([FromBody] EmployeeCreateDto employeeCreate)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var result = await _userService.RegisterEmployee(employeeCreate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                apiResponse.StatusCode = 400;
                apiResponse.Message = ex.Message;
                apiResponse.Success = false;
            }
            return BadRequest(apiResponse); 
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginTask([FromBody] LoginDto loginDto)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var result = await _userService.Login(loginDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                apiResponse.StatusCode = 400;
                apiResponse.Message = ex.Message;
                apiResponse.Success = false;
            }
            return BadRequest(apiResponse); 
        }
        
        [HttpPost]//Only for registered users
        [Route("Delete")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete()
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                string id = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var result = await _userService.Delete(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                apiResponse.StatusCode = 400;
                apiResponse.Message = ex.Message;
                apiResponse.Success = false;
            }
            return BadRequest(apiResponse); 
        }
        
        [HttpPost]
        [Route("Delete")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteUser()
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                string id = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var result = await _userService.Delete(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                apiResponse.StatusCode = 400;
                apiResponse.Message = ex.Message;
                apiResponse.Success = false;
            }
            return BadRequest(apiResponse); 
        }

        [HttpPut]
        [Route("UpdateRole")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateRole(int id)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var result = await _userService.UpdateRole(id);
                return Ok(result);
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
        [Route("{userid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserBack([FromRoute] int userid)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var result = await _userService.UpdateRole(userid);
                return Ok(result);
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
        [Route("getuser")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserById(UserGetByIdDto userGetDto)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var result = await _userService.UpdateRole(userGetDto);
                return Ok(result);
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
        [Route("Getallusers")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserById()
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var result = await _userService.GetAllUsers();
                return Ok(result);
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
