using BiblioTar.Entities;
using BiblioTar.Context;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using BiblioTar.DTOs;
using System.Security.Claims;
using BiblioTar.Entities.Enums;

namespace BiblioTar.Service
{

    public interface IReservationService
    {
        Task<int> CreateReservation(ReservationCerateDto reservation);
        Task<int> CreateReservationLoggedIn(ReservationCerateLoggedinDto reservation);
        Task<List<BookGetDto>> GetAvailableBooksForReservation();
        Task<List<ReservationDto>> GetMyReservations();
    }
    public class ReservationService: IReservationService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public ReservationService(AppDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<int> CreateReservationLoggedIn(ReservationCerateLoggedinDto reservation)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == reservation.BookId)
                ?? throw new Exception("Nincs ilyen című könyv, vagy nem létezik.");

            if (book.Status == Book.StatusEnum.unavailable)
            {
                throw new Exception("Ez a könyv már le van foglalva.");
            }
            string userIdString = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) 
                                  ?? throw new Exception("A foglaláshoz bejelentkezés szükséges.");
            
            ReservationLOggedCreateDto reservationCreateDto = new ReservationLOggedCreateDto()
            {
                UserId = int.Parse(userIdString),
                BookId = book.Id,
                ReservationDate = DateTime.Now,
                Status = ReservationStatus.Active 
            };
            
            book.Status = Book.StatusEnum.reserved;
            _context.Books.Update(book);

            var reservationEntity = _mapper.Map<Reservation>(reservationCreateDto);
            await _context.Reservations.AddAsync(reservationEntity);
            await _context.SaveChangesAsync();

            return reservationEntity.Id; 
        }
        
        public async Task<int> CreateReservation(ReservationCerateDto reservation)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == reservation.BookId)
                       ?? throw new Exception("Nincs ilyen című könyv, vagy nem létezik.");

            if (book.Status == Book.StatusEnum.unavailable)
            {
                throw new Exception("Ez a könyv már le van foglalva.");
            }

            Reservation L_res = new Reservation
            {
                Email = reservation.Email,
                BookId = book.Id,
                ReservationDate = DateTime.Now,
                Status = ReservationStatus.Active
            };
            
            book.Status = Book.StatusEnum.reserved;
            _context.Books.Update(book);
            
            await _context.Reservations.AddAsync(L_res);
            await _context.SaveChangesAsync();

            return L_res.Id; 
        }

        public async Task<List<BookGetDto>> GetAvailableBooksForReservation()
        {
            var availableBooks = await _context.Books
                .Where(b => b.Status == Book.StatusEnum.available) 
                .ToListAsync();

            return _mapper.Map<List<BookGetDto>>(availableBooks);
        }

        public async Task<List<ReservationDto>> GetMyReservations()
        {
            var userIdString = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                
                throw new Exception("Felhasználó azonosító nem található a foglalások lekérdezéséhez.");
            }
            var userId = int.Parse(userIdString);

            var userReservations = await _context.Reservations
                .Where(r => r.UserId == userId)
                .Include(r => r.Book) 
                .OrderByDescending(r => r.ReservationDate) 
                .ThenBy(r => r.Status) 
                .ToListAsync();

            
            return _mapper.Map<List<ReservationDto>>(userReservations);
        }
    }
}
