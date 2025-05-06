using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioTar.Entities
{
    public class User
    {
        public int Id { get; set; } 
        public string Email { get; set; } //Tervezet szerin alternáló kulcs lesz
        public string Name { get; set; }
        public string Password { get; set; }
        
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        
        public bool IsEnabled { get; set; } = true; //true == engedélyezett, false == letiltott
        
        public Address Address { get; set; } 

        [Flags]
        public enum RoleEnums
        {
            Customer=1,
            Librarian=2,
            Administrator=4
        }
        public RoleEnums Roles { get; set; } = RoleEnums.Customer;
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
        public List<Borrow> Borrows { get; set; } = new List<Borrow>();

    }
}
