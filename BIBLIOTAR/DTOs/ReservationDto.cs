using BiblioTar.Entities;
using BiblioTar.Entities.Enums;

namespace BiblioTar.DTOs;

public class ReservationDto
{
    public int Id { get; set; }
    public string? Email { get; set; } 
    public int? UserId { get; set; }
    public int BookId { get; set; }
    public string BookTitle { get; set; } 
    public DateTime ReservationDate { get; set; }
    public ReservationStatus Status { get; set; }
}

public class ReservationLOggedCreateDto
{
    public int Id { get; set; }
    public string? Email { get; set; } 
    public int? UserId { get; set; }
    public int BookId { get; set; }
    public DateTime ReservationDate { get; set; }
    public ReservationStatus Status { get; set; }
}


public class ReservationCerateDto
{
    public string Email { get; set; } 
    public int BookId { get; set; }
}

public class ReservationCerateLoggedinDto
{
    public int BookId { get; set; }
}
