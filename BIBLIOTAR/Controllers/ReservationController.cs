using Microsoft.AspNetCore.Mvc;
using BiblioTar.Entities;
using System.Linq;
using BiblioTar.Context;
using BiblioTar.Service;
using Microsoft.AspNetCore.Authorization;
using BiblioTar.Entities.Enums;

namespace BiblioTar.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly AppDbContext _dbContext;

        public ReservationController(IReservationService reservationService, AppDbContext dbContext)
        {
            _reservationService = reservationService;
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("create")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateReservation([FromQuery] int bookId, [FromQuery] string? email = null)
        {
            try
            {
                var book = _dbContext.Books.FirstOrDefault(b => b.Id == bookId && b.Status == Book.StatusEnum.available);
                if (book == null)
                {
                    return BadRequest(new { message = "A megadott könyv nem elérhető." });
                }

                int? userId = null;
                if (User.Identity?.IsAuthenticated == true)
                {
                    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                    if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int parsedId))
                    {
                        userId = parsedId;
                    }
                }
                else if (string.IsNullOrWhiteSpace(email))
                {
                    return BadRequest(new { message = "Email megadása kötelező anonim foglalás esetén." });
                }

                var reservation = new Reservation
                {
                    BookId = book.Id,
                    UserId = userId,
                    Email = email,
                    ReservationDate = DateTime.Now,
                    Status = ReservationStatus.Active
                };

                _dbContext.Reservations.Add(reservation);
                await _dbContext.SaveChangesAsync();

                book.Status = Book.StatusEnum.unavailable;
                await _dbContext.SaveChangesAsync();

                return Ok(new { message = "Foglalás sikeresen létrehozva.", reservation });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Hiba történt a foglalás során: {ex.Message}" });
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
        [Authorize]
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
