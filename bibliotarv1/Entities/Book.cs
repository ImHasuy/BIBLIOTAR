namespace bibliotar.Entities
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public int PublishedYear { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public bool Borrowable { get; set; }

        public ICollection<Loan> Loans { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
}
