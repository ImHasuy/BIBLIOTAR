using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioTar.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        public string? Email { get; set; } //Lentebb kifejtve 
        // ALternalokulcsnak tervezem, egyelore nem lett felepitve a DB contextben
        //Nem áll egyelőre így semmivel sem kapcsolatban, amennyibben szükséges, hogy valami key legyen, utána nezek
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("Book")]
        public int BookId { get; set; }
        public DateTime ReservationDate { get; set; }
        public enum ReservationStatus { Active, Completed, Canceled }
        public ReservationStatus Status { get; set; }

        public User? User { get; set; }

        public Book? Book { get; set; }

    }
}
