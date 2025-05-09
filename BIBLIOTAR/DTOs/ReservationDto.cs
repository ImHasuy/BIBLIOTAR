<<<<<<< Updated upstream
﻿using BiblioTar.Entities;
using BiblioTar.Entities.Enums;

namespace BiblioTar.DTOs;

public class ReservationDto
{
    public int Id { get; set; }
    public string? Email { get; set; } 
    public int? UserId { get; set; }
    public int BookId { get; set; }
    public DateTime ReservationDate { get; set; }
    public ReservationStatus Status { get; set; }
}
=======
﻿using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioTar.DTOs
{
    public class ReservationDto
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("Book")]
        public int BookId { get; set; }
        public DateTime ReservationDate { get; set; }=DateTime.Now;
    }
}
>>>>>>> Stashed changes
