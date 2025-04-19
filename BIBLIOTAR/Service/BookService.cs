using AutoMapper;
using BiblioTar.Context;
using BiblioTar.DTOs;
using BiblioTar.Entities;
using Microsoft.Build.Framework;

namespace BiblioTar.Service
{
    public interface IBookService
    {
        Task<int>RegisterBook(BookCreateDto bookCreateDto);
        Task<bool> DeleteBook(int id);
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
            var book = await _appDbContext.Books.FindAsync(id);
            if(book == null)
            {
                throw new KeyNotFoundException("Nem található");
            }
            _appDbContext.Books.Remove(book);
            _appDbContext.SaveChanges();
            return true;
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
