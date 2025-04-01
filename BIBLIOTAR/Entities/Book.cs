namespace BiblioTar.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Quality {  get; set; }
        public string ISBN { get; set; }
        public string Category {  get; set; }
        public DateTime PublshDate { get; set; }

    }
}
