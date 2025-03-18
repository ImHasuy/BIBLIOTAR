namespace bibliotar.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }


        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public enum Roles { User, Libarian, Admin }
        public Roles Role { get; set; }
        public DateTime RegistrationDate { get; set; }

        public ICollection<Loan> Loans { get; set; }
        public ICollection<Reservation> Reservations { get; set; }

    }
}
