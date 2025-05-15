using Microsoft.AspNetCore.Mvc;
using BiblioTar.Entities;
using System.Linq;
using BiblioTar.Context;
using BiblioTar.DTOs;
using BiblioTar.Service;
using Microsoft.AspNetCore.Authorization;
using BiblioTar.Entities.Enums;

namespace BiblioTar.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost]
        [Route("create")]
        [Authorize(Policy = "AllUserPolicy")]
        public async Task<IActionResult> CreateReservationLoggedIn(ReservationCerateLoggedinDto reservation)
        {
            
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var response = await _reservationService.CreateReservationLoggedIn(reservation);
                apiResponse.Data = response;
                apiResponse.Message = "Reservation created successfully";
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
        [Route("createAsGuest")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateReservation(ReservationCerateDto reservation )
        {
            
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var response = await _reservationService.CreateReservation(reservation);
                apiResponse.Data = response;
                apiResponse.Message = "Reservation created successfully";
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
        [Route("MyReservations")]
        [Authorize(Policy = "AllUserPolicy")]
        public async Task<IActionResult> GetMyReservations()
        {
            try
            {
                var myReservations = await _reservationService.GetMyReservations();
                return Ok(myReservations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Hiba történt a foglalások lekérdezése közben: {ex.Message}" });
            }
        }
    }
}
