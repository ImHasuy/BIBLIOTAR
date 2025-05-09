using AutoMapper;
using BiblioTar.Context;
using BiblioTar.DTOs;
using BiblioTar.Entities;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;

namespace BiblioTar.Service
{
    public interface IBookService
    {
        Task<int>RegisterBook(BookCreateDto bookCreateDto);
        Task<bool> DeleteBook(int id);
        Task<List<BookGetDto>> GetAllBooks();
        Task<BookGetDto> GetBook(string title);
    }

    public class BookService : IBookService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;


        public BookService(AppDbContext appDbContext,IMapper mapper)
        {
            _appDbContext = appDbContext;     
            _mapper = mapper;
        }

        public async Task<bool> DeleteBook(int id )
        {
            var book = await _appDbContext.Books.FindAsync(id) ?? throw new Exception("Nem található");
            _appDbContext.Books.Remove(book);
            _appDbContext.SaveChanges();
            return true;
        }

        public async Task<List<BookGetDto>> GetAllBooks()
        {
            var books = await _appDbContext.Books.ToListAsync();
            return _mapper.Map<List<BookGetDto>>(books);
        }

        public async Task<BookGetDto> GetBook(string title)
        {
            var book= await _appDbContext.Books.FirstOrDefaultAsync(x => x.Title == title) ?? throw new Exception("A könyv ne mtalálható");
            return _mapper.Map<BookGetDto>(book);
        }

        public async Task<int> RegisterBook(BookCreateDto bookCreateDto)
        {
            var book=_mapper.Map<Book>(bookCreateDto);
            await _appDbContext.Books.AddAsync(book);
            await _appDbContext.SaveChangesAsync();
            return book.Id;
        }
    }
}
