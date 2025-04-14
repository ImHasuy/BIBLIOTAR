using BiblioTar.Context;
using BiblioTar.DTOs;
using BiblioTar.Entities;

namespace BiblioTar.Service
{
    public interface IAddressService
    {
         Task<int> CreateAsyncAddress(AddressCreateDto addressCreateDto);
    }


    public class AddressService:IAddressService
    {
        private readonly AppDbContext _context;
        public AddressService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<int> CreateAsyncAddress(AddressCreateDto addressCreateDto)
        {
            var address = new Address
            {
                Country = addressCreateDto.Country,
                ZipCode = addressCreateDto.ZipCode,
                City = addressCreateDto.City,
                Street = addressCreateDto.Street,
                HouseNumber = addressCreateDto.HouseNumber,
            };
            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();
            return address.Id;
        }
    }
}
