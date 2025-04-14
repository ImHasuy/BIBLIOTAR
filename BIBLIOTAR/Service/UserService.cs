using AutoMapper;
using BiblioTar.Context;
using BiblioTar.Controllers;
using BiblioTar.DTOs;
using BiblioTar.Entities;
using Microsoft.EntityFrameworkCore;

namespace BiblioTar.Service
{


    public interface IUserService
    {
        Task<int> CreateAsync(UserCreateDto userCreateDto);
    }


    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public UserService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<int> CreateAsync(UserCreateDto userCreateDto)
        {
            var user = _mapper.Map<User>(userCreateDto);
    
            var address =  new Address
            { 
                ZipCode = userCreateDto.ZipCode,
                City = userCreateDto.City,
                Street = userCreateDto.Street,
                HouseNumber = userCreateDto.HouseNumber,
                Country = userCreateDto.Country
            };



            var temp = await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();//Ha midnen igaz, menteni kell, hogy az id-t megkapja
            user.AddressId = temp.Entity.Id;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user.Id;


        }
    }
}
