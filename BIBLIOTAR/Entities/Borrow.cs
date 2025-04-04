namespace BiblioTar.Entities
{
    public class Borrow
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public int RenewalsLeft { get; set; } = 2;



        public User User { get; set; }
        public Book Book { get; set; }


    }
}
