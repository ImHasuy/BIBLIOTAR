namespace BiblioTar.Entities
{
    public class Fine
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Amount { get; set; }
        public int BorrowId { get; set; }
        public bool PaidStatus { get; set; } // false == még függő, true == kiegyenlítve 
        public DateTime IssuedDate { get; set; }


        public Borrow borrow { get; set; }
        public User user { get; set; }
    }
}
