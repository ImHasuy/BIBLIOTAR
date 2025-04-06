using BiblioTar.Entities;
using BiblioTar.Context;
using System.Collections.Generic;
using System.Linq;

namespace BiblioTar.Service
{
    public class ReservationService
    {
        private readonly AppDbContext _context;

        public ReservationService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Reservation> GetReservations()
        {
            return _context.Reservations.ToList();
        }

        public Reservation GetReservation(int id)
        {
            return _context.Reservations.Find(id);
        }

        public Reservation CreateReservation(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            _context.SaveChanges();
            return reservation;
        }

        public void UpdateReservation(int id, Reservation reservation)
        {
            if (id == reservation.UserId)
            {
                _context.Entry(reservation).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public void DeleteReservation(int id)
        {
            var reservation = _context.Reservations.Find(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
                _context.SaveChanges();
            }
        }
    }
}

