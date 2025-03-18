using bibliotar.Context;
using bibliotar.Entities;

namespace bibliotar.Services
{
    public interface IBibliotarService
    {
        List<Book> List();
    }


    public class BookService : IBibliotarService
    {
        private readonly AppDbContext _context;

        public BookService(AppDbContext context)
        {
            _context = context;
        }


        public List<Book> List()
        {
            return _context.Books.ToList();
        }


    }
}
