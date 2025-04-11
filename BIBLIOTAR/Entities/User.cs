using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioTar.Entities
{
    public class User
    {
        public int Id { get; set; } 
        public string Email { get; set; } //Tervezet szerin alternáló kulcs lesz
        public string Name { get; set; }
 
        public string Password { get; set; }

        [ForeignKey("Address")]
        public int? AddressId { get; set; } //Egyben a foreign key is
        

        public DateTime RegistrationDate { get; set; }


        public Address? Address { get; set; }
        public List<Role>? Roles { get; set; }
        public List<Reservation>? Reservations { get; set; }
        public List<Borrow>? Borrows { get; set; }

    }
}
