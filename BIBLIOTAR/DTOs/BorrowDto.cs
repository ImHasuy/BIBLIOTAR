using BiblioTar.Entities.Enums;

namespace BiblioTar.DTOs;

public class BorrowDto
{
    public int Id { get; set; }
    public int UserId { get; set; }  // Egyben a foreign key is
    public int BookId { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime DueDate { get; set; }
    public int RenewalsLeft { get; set; } 
    public BorrowStatus borrowStatus { get; set; }  
}