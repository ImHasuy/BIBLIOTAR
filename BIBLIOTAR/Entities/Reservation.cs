using BiblioTar.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioTar.Entities
{
    public class Reservation
    {
<<<<<<< Updated upstream
        public int Id { get; set; }
        public string? Email { get; set; } //Amennyiben a felhasználó nem regisztrált, akkor az email címét itt tároljuk el

=======
        public int Id { get; set; }=new int();
        public string? Email { get; set; } 
        //Lentebb kifejtve 
        // ALternalokulcsnak tervezem, egyelore nem lett felepitve a DB contextben
        //Nem áll egyelőre így semmivel sem kapcsolatban, amennyibben szükséges, hogy valami key legyen, utána nezek
>>>>>>> Stashed changes
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
