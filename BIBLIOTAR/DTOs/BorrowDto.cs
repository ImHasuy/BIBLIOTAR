using BiblioTar.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

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



public class BorrowCreateDto
{
    public int UserId { get; set; }
    public int BookId { get; set; }
    public int BorrowPeriodInDays { get; set; } 
}
public class BorrowCreateForUserDto
{
    public int UserId { get; set; }  
    public int BookId { get; set; }
    public int BorrowPeriod { get; set; }
}

public class BorrowInputDto
{
    public int Id { get; set; }
}
public class BorrowExtendDto
{
    public int Id { get; set; }
    public int BorrowPeriodExtend { get; set; }
}

public class BorrowStatusModifyDto
{
    public int Id { get; set; }
    public BorrowStatus StatusModifyer { get; set; }
}