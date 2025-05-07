using AutoMapper;
using BiblioTar.Context;
using BiblioTar.DTOs;
using BiblioTar.Entities;
using Microsoft.EntityFrameworkCore;

namespace BiblioTar.Service
{
    public interface IBorrowService
    {
        Task<int> CreateBorrow(BorrowCreateDto borrowCreateDto);
        Task<string> ExtendBorrowPeriod(BorrowExtendDto borrowExtendDto);
        Task<string> ModifyBorrowStatus(BorrowStatusModifyDto borrowStatusModifyDto);
        Task<BorrowDto> GetBorrowByBookId(BorrowInputDto borrowInputDto);
        Task<List<BorrowDto>> GetAllBorrowForUser(BorrowInputDto borrowInputDto);
    }

    public class BorrowService : IBorrowService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public BorrowService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<int> CreateBorrow(BorrowCreateDto borrowCreateDto)
        {

            var book = await _context.Books.FirstOrDefaultAsync(c => c.Id == borrowCreateDto.BookId) ?? throw new Exception($"No book found with  {borrowCreateDto.BookId} Id");
            var user = await _context.Users.FirstOrDefaultAsync(c => c.Id == borrowCreateDto.UserId) ?? throw new Exception($"No User found with {borrowCreateDto.UserId} Id");

            if (book.Status == Book.StatusEnum.unavailable)
            {
                throw new Exception("The Book is not available");
            }

            var temp = new Borrow
            {
                UserId = user.Id,
                BookId = book.Id,
                DueDate = DateTime.Now.AddDays(borrowCreateDto.BorrowPeriodInDays),
                borrowStatus = 0
            };
            book.Status = Book.StatusEnum.unavailable;
            await _context.Borrows.AddAsync(temp);
            await _context.SaveChangesAsync();

            return temp.Id;
        }


        //Ide is borrow id
        public async Task<string> ExtendBorrowPeriod(BorrowExtendDto borrowExtendDto)
        {
            var borrow = await _context.Borrows.FirstOrDefaultAsync(c=> c.Id==borrowExtendDto.Id) ?? throw new Exception($"No borrow find with {borrowExtendDto.Id}");
            if(borrow.RenewalsLeft < 1)
            {
                throw new Exception("No more renewals left.");
            }
            borrow.DueDate = borrow.DueDate.AddDays(borrowExtendDto.BorrowPeriodExtendInDays);
            borrow.RenewalsLeft -= 1;

            _context.Borrows.Update(borrow);
            await _context.SaveChangesAsync();
            return $"Borrow period extended succesfully";
        }

        public async Task<List<BorrowDto>> GetAllBorrowForUser(BorrowInputDto borrowInputDto)
        {
            var borrows = await _context.Borrows.Where(c => c.UserId == borrowInputDto.Id).ToListAsync() ?? throw new Exception($"No borrows find for {borrowInputDto.Id} UserId");
            return _mapper.Map<List<BorrowDto>>(borrows);
        }

        //Ide bookid
        public async Task<BorrowDto> GetBorrowByBookId(BorrowInputDto borrowInputDto)
        {
            var borrow = await _context.Borrows.FirstOrDefaultAsync(c=>c.BookId == borrowInputDto.Id) ?? throw new Exception($"No borrow find for {borrowInputDto.Id} BookId");
            return _mapper.Map<BorrowDto>(borrow);


        }

        //Ide a Borrow Id
        public async Task<string> ModifyBorrowStatus(BorrowStatusModifyDto borrowStatusModifyDto)
        {
            
            var borrow = await _context.Borrows.Include(k=>k.Book).FirstOrDefaultAsync(c => c.Id == borrowStatusModifyDto.Id) ?? throw new Exception($"No borrow find for {borrowStatusModifyDto.Id} Borrow");
            borrow.borrowStatus = borrowStatusModifyDto.StatusModifyer;
            borrow.Book.Status = Book.StatusEnum.available;
            _context.Borrows.Update(borrow);
            await _context.SaveChangesAsync();
            return $"Borrow status succesfully modified to {borrowStatusModifyDto.StatusModifyer}";
        }
    }
}
