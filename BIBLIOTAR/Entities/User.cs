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

        public enum RoleEnums
        {
            Customer,
            Librarian,
            Administrator
        }
        public RoleEnums Roles { get; set; } = RoleEnums.Customer;
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
        public List<Borrow> Borrows { get; set; } = new List<Borrow>();

    }
}
