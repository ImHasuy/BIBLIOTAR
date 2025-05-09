namespace BiblioTar.Entities
{
    public class Book
    {
        public int Id { get; set; }
<<<<<<< Updated upstream
        public enum StatusEnum { available, unavailable }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Quality { get; set; }
        public string ISBN { get; set; }
        public string Category { get; set; }
=======
        public enum StatusEnum { available, unalvilable }
        public string Title { get; set; }= string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Quality { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
>>>>>>> Stashed changes
        public DateTime PublishDate { get; set; }
        public StatusEnum Status { get; set; }=StatusEnum.available;
    }
}
