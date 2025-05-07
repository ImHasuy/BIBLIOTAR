namespace BiblioTar.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public enum StatusEnum { available, unavailable }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Quality { get; set; }
        public string ISBN { get; set; }
        public string Category { get; set; }
        public DateTime PublishDate { get; set; }
        public StatusEnum Status { get; set; }=StatusEnum.available;
    }
}
