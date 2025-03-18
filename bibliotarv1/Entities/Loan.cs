using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bibliotar.Entities
{
    public class Loan
    {
       
        public int LoanId { get; set; }
        
        public int UserId { get; set; }
       
        public int BookId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int RenewalsLeft { get; set; } = 2;
        public decimal Fine { get; set; } = 0;


        public User User { get; set; }


        public Book Book { get; set; }

    }
}
