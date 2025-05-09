using BiblioTar.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioTar.Entities
{
    public class Borrow
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }  // Egyben a foreign key is

        [ForeignKey("Book")]
        public int BookId { get; set; }
        public DateTime BorrowDate { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; }
        public int RenewalsLeft { get; set; } = 2;
        public BorrowStatus borrowStatus { get; set; } = BorrowStatus.Borrowed; 


        public User? User { get; set; }
        public Fine? Fine { get; set; }
        public Book? Book { get; set; }

    }
}
