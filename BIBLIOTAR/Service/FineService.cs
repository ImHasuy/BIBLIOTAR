using BiblioTar.Context;
using BiblioTar.DTOs;
using BiblioTar.Entities;

namespace BiblioTar.Service
{

    public interface IFineService
    {
        Task<int> CreateAsync(FineCreateDto fineCreateDto);

    }

    public class FineService : IFineService
    {

        //Dep. injection
        private readonly AppDbContext _context;

        public FineService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<int> CreateAsync(FineCreateDto fineCreateDto)
        {
            var fine = new Fine
            {
                Amount = fineCreateDto.Amount,
                BorrowId = fineCreateDto.BorrowId,
                IssuedDate = DateTime.Now,
                PaidStatus = false, // default value
                UserId = fineCreateDto.UserId
            };

            await _context.Fines.AddAsync(fine);
            await _context.SaveChangesAsync();
            return fine.Id;
        }

        
    }
}
