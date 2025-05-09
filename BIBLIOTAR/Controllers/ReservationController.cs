using Microsoft.AspNetCore.Mvc;
using BiblioTar.Entities;
using System.Collections.Generic;
using System.Linq;
using BiblioTar.Context;
using BiblioTar.Service;

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
        public async Task<IActionResult> CreateReservation(string title)
        {
            try
            {
                var result = await _reservationService.CreateReservation(title);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}

