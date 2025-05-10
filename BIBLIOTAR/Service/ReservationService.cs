using BiblioTar.Entities;
using BiblioTar.Context;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using BiblioTar.DTOs;
using System.Security.Claims;

namespace BiblioTar.Service
{

    public interface IReservationService
    {
        Task<int> CreateReservation(string title);
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
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Title == title) ?? throw new Exception("Nincs ilyne könyv");
            if (book.Status == Book.StatusEnum.available) // Use the enum value directly
            {
                ReservationDto reservation = new ReservationDto
                {
                    UserId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    BookId = book.Id,
                    ReservationDate = DateTime.Now,
                };
                book.Status = Book.StatusEnum.unavailable;
                _context.Books.Update(book);
                await _context.Reservations.AddAsync(_mapper.Map<Reservation>(reservation));
                await _context.SaveChangesAsync();
                return reservation.Id;
            }
            else
            {
                throw new Exception("A könyv már foglalt");
            }
        }


    }
}

