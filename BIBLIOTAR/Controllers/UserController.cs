using System.Security.Claims;
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
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("create")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserCreateDto userCreateDto)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var response = await _userService.RegisterCustomer(userCreateDto);
                apiResponse.Data = response;
                apiResponse.Message = "User created successfully";
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

        [HttpPost]
        [Route("createEmployee")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterEmployee([FromBody] EmployeeCreateDto employeeCreate)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var response = await _userService.RegisterEmployee(employeeCreate);
                apiResponse.Data = response;
                apiResponse.Message = "User created successfully";
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

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginTask([FromBody] LoginDto loginDto)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var response = await _userService.Login(loginDto);
                apiResponse.Data = response;
                apiResponse.Message = "User logged in successfully";
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
        
        [HttpDelete]//Only for registered users
        [Route("Delete")]
        [Authorize(Policy = "AllUserPolicy")]
        public async Task<IActionResult> Delete()
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                string id = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                apiResponse.Message = await _userService.Delete(id);
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
        
        [HttpDelete]
        [Route("DeleteUserById")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteUser(UserInputDto userInputDto)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                apiResponse.Message = await _userService.Delete(userInputDto.Id.ToString());
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

        [HttpPut]
        [Route("UpdateRole")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateRole(UserUpdateDto userUpdateDto)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                apiResponse.Message = await _userService.UpdateRole(userUpdateDto);
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
        [Route("{userid}")]
        [Authorize(Policy = "StaffPolicy")]
        public async Task<IActionResult> GetUserBack([FromRoute] string userid)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                apiResponse.Data = await _userService.GetUserById(userid);
                apiResponse.Message = "User found successfully";
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
        [Route("getuser")]
        [Authorize(Policy = "AllUserPolicy")]
        public async Task<IActionResult> GetUserById()
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                string id = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                apiResponse.Data = await _userService.GetUserById(id);
                apiResponse.Message = "User found successfully";
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
        [Route("Getallusers")]
        [Authorize(Policy = "StaffPolicy")]
        public async Task<IActionResult> GetAllUsers()
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                apiResponse.Data = await _userService.GetAllUsers();
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


        
        [HttpPut]
        [Route("UpdateInformations")] //Az inputnal ki kell szedni a placeholder stringeket, mert a bentebbi Addressnél már nem tudja kezelni
        [Authorize(Policy = "AllUserPolicy")]
        public async Task<IActionResult> UpdateInformations(UserUpdateInformationDto updateInformationDto)
        {
            var temp = _mapper.Map<UserDtoToUpdateFunc>(updateInformationDto);
            temp.Id = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                apiResponse.Message = await _userService.UpdateInformations(temp);
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
