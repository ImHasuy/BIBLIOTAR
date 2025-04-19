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
        public async Task<int> RegisterBook(BookCreateDto bookCreateDto)
        {
            var book=_mapper.Map<Book>(bookCreateDto);
            await _appDbContext.Books.AddAsync(book);
            await _appDbContext.SaveChangesAsync();
            return book.Id;
        }
    }
}
