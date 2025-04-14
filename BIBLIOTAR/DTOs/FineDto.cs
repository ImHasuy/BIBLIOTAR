namespace BiblioTar.DTOs
{
    public class FineCreateDto
    {
        public int UserId { get; set; }
        public int BorrowId { get; set; }
        public int Amount { get; set; }
    }

    public class FineGetDto
    {
        public int Amount { get; set; }
        public int BorrowId { get; set; }
        public bool PaidStatus { get; set; } // false == még függő, true == kiegyenlítve 
        public DateTime IssuedDate { get; set; }
    }
}
