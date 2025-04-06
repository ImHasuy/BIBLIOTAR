using BiblioTar.ConnectionTables;

namespace BiblioTar.Entities
{
    public class User
    {

        public required string Email { get; set; } //Egyben a primary key is 
        public string Name { get; set; }
 
        public string Password { get; set; }
        public int AddressId { get; set; } //Egyben a foreign key is
        

        public DateTime RegistrationDate { get; set; }

        public Address? Address { get; set; }
        public List<UserRoles>? UserRoles { get; set; }
        public List<Reservation>? Reservations { get; set; }
        public List<Borrow>? Borrows { get; set; }

    }
}
