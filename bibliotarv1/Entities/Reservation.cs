namespace bibliotar.Entities
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime ReservationDate { get; set; }
        public enum ReservationStatus { Active, Completed, Canceled }
        public ReservationStatus Status { get; set; }


        public User User { get; set; }


        public Book Book { get; set; }

    }
}
