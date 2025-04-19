using static BiblioTar.Entities.Book;

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
        public int Status { get; set; }
    }



    public class BookGetDto
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Quality { get; set; }
        public string ISBN { get; set; }
        public string Category { get; set; }
        public DateTime PublishDate { get; set; }
        public int Status { get; set; }
    }
}

