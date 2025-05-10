using Microsoft.AspNetCore.Mvc;
using BiblioTar.Entities;
using System.Collections.Generic;
using System.Linq;
using BiblioTar.Context;
using BiblioTar.Service;
using BiblioTar.DTOs; 
using Microsoft.AspNetCore.Authorization; 

namespace BiblioTar.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost]
        [Route("{title}")]
        [Authorize]
        public async Task<IActionResult> CreateReservation(string title)
        {   
            try
            {
                var resultId = await _reservationService.CreateReservation(title);
                
                return Ok(new { ReservationId = resultId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("AvailableBooks")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAvailableBooks()
        {
            try
            {
                var books = await _reservationService.GetAvailableBooksForReservation();
                return Ok(books);
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, new { message = $"Szerverhiba történt az elérhető könyvek lekérdezése közben: {ex.Message}" });
            }
        }

        [HttpGet]
        [Route("MyReservations")] 
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