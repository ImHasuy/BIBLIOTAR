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
        Task<int> CreateReservation(string title);
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

        public async Task<int> CreateReservation(string title)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Title == title)
                ?? throw new Exception("Nincs ilyen című könyv, vagy nem létezik.");

            var userIdString = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                
                throw new Exception("A foglaláshoz bejelentkezés szükséges.");
            }
            var userId = int.Parse(userIdString);

            
            bool alreadyReservedByUser = await _context.Reservations
                .AnyAsync(r => r.BookId == book.Id && r.UserId == userId && r.Status == ReservationStatus.Active);

            if (alreadyReservedByUser)
            {
                throw new Exception("Már van aktív foglalásod erre a könyvre.");
            }

            ReservationDto reservationDto = new ReservationDto
            {
                UserId = userId,
                BookId = book.Id,
                ReservationDate = DateTime.Now,
                Status = ReservationStatus.Active 
            };

            
            if (book.Status == Book.StatusEnum.available)
            {
                book.Status = Book.StatusEnum.unavailable;
                _context.Books.Update(book);
            }
            

            var reservationEntity = _mapper.Map<Reservation>(reservationDto);
            await _context.Reservations.AddAsync(reservationEntity);
            await _context.SaveChangesAsync();

            return reservationEntity.Id; 
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
