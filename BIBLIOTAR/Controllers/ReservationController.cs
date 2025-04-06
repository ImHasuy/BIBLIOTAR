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
        private readonly ReservationService _reservationService;

        public ReservationController(ReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Reservation>> GetReservations()
        {
            return Ok(_reservationService.GetReservations());
        }

        [HttpGet("{id}")]
        public ActionResult<Reservation> GetReservation(int id)
        {
            var reservation = _reservationService.GetReservation(id);

            if (reservation == null)
            {
                return NotFound();
            }

            return Ok(reservation);
        }

        [HttpPost]
        public ActionResult<Reservation> CreateReservation(Reservation reservation)
        {
            var createdReservation = _reservationService.CreateReservation(reservation);
            return CreatedAtAction(nameof(GetReservation), new { id = createdReservation.UserId }, createdReservation);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateReservation(int id, Reservation reservation)
        {
            if (id != reservation.UserId)
            {
                return BadRequest();
            }

            _reservationService.UpdateReservation(id, reservation);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReservation(int id)
        {
            _reservationService.DeleteReservation(id);
            return NoContent();
        }
    }
}

