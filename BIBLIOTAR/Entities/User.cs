namespace BiblioTar.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Address Address { get; set; }

        public DateTime RegistrationDate { get; set; }

        public List<Role> Roles { get; set; }
        public List<Reservation> Reservations { get; set; }
        public List<Borrow> Borrows { get; set; }



    }
}
