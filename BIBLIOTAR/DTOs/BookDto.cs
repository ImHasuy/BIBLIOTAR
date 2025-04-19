namespace BiblioTar.DTOs
{
    public class BookCreateDto
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Quality { get; set; }
        public string ISBN { get; set; }
        public string Category { get; set; }
        public DateTime PublishDate { get; set; }
        public enum Status { available, unalvilable }
    }



    public class BookGetDto
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Quality { get; set; }
        public string ISBN { get; set; }
        public string Category { get; set; }
        public DateTime PublishDate { get; set; }
        public enum Status { available, unalvilable }
    }
}

