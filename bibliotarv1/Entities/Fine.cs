namespace bibliotar.Entities
{
    public class Fine
    {
        public int FineId { get; set; }
        public int UserId { get; set; }
        public int LoanId { get; set; }
        public decimal Amount { get; set; }
        public bool PaidStatus { get; set; }
        public DateTime IssuedDate { get; set; }


        public User User { get; set; }
        public Loan Loan { get; set; }
    }
}
