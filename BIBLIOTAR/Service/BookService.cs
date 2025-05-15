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
        Task<bool> DeleteBook(int bookId);
        Task<List<BookGetDto>> GetAllBooks();
        Task<BookGetDto> GetBook(string title);
        Task<string> UpdateBook(BookUpdateDto bookUpdateDto);
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

        public async Task<bool> DeleteBook(int BookId)
        {
            var book = await _appDbContext.Books.FindAsync(BookId) ?? throw new Exception("Nem található");
            _appDbContext.Books.Remove(book);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<BookGetDto>> GetAllBooks()
        {
            var books = await _appDbContext.Books.ToListAsync();
            return _mapper.Map<List<BookGetDto>>(books);
        }

        public async Task<BookGetDto> GetBook(string title)
        {
            var book= await _appDbContext.Books.FirstOrDefaultAsync(x => x.Title == title) ?? throw new Exception("A könyv nem található");
            return _mapper.Map<BookGetDto>(book);
        }

        public async Task<int> RegisterBook(BookCreateDto bookCreateDto)
        {
            var book=_mapper.Map<Book>(bookCreateDto);
            await _appDbContext.Books.AddAsync(book);
            await _appDbContext.SaveChangesAsync();
            return book.Id;
        }
        
        public async Task<string> UpdateBook(BookUpdateDto bookUpdateDto)
        {
            
            var book = await _appDbContext.Books.FindAsync(bookUpdateDto.Id) ?? throw new Exception("A könyv nem található");
            book.Title = bookUpdateDto.Title;
            book.Author = bookUpdateDto.Author;
            book.Category = bookUpdateDto.Category;
            book.ISBN = bookUpdateDto.ISBN;
            book.PublishDate = bookUpdateDto.PublishDate;
            book.Quality = bookUpdateDto.Quality;
            book.Status = (Book.StatusEnum)bookUpdateDto.Status;
            
            _appDbContext.Books.Update(book);
            await _appDbContext.SaveChangesAsync();
            return $"The book with id {book.Id} successfully updated";
        }
    }
}
