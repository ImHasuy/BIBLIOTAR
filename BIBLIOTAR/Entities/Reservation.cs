using BiblioTar.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioTar.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        public string? Email { get; set; } //Amennyiben a felhasználó nem regisztrált, akkor az email címét itt tároljuk el

        [ForeignKey("User")]
        public int? UserId { get; set; } //Ha a felhasználó nem regisztrált, akkor egy anonymus userhez lesz kapcsolva, és itt lesz eltarolva az email.
        [ForeignKey("Book")]
        public int BookId { get; set; }
        public DateTime ReservationDate { get; set; }
        
        public ReservationStatus Status { get; set; }

        public User? User { get; set; }

        public Book Book { get; set; }

    }
}
