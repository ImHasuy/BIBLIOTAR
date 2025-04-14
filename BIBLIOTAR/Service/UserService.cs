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
            //    new User
            //{
            //    Email = userCreateDto.Email,
            //    Name = userCreateDto.Name,
            //    Password = userCreateDto.Password,
            //    RegistrationDate = DateTime.Now,
            //    AddressId = null
            //};

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

            user.Roles = new List<Role>();

            if(userCreateDto.RoleIds != null)
            {
                foreach (var rRole in userCreateDto.RoleIds)
                {
                    var ValidRole = await _context.Roles.FirstOrDefaultAsync(r => r.Id == rRole); // If its exist, it retuns it
                    if(ValidRole != null)
                    {
                        user.Roles.Add(ValidRole);
                    }
                }
            }

            if (!user.Roles.Any())
            {
                user.Roles.Add(await GetDefaultCustomerRoleAsync());
            }


            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user.Id;


        }

        private async Task<Role> GetDefaultCustomerRoleAsync()
        {
            var customerRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Customer");
            if (customerRole == null)
            {
                customerRole = new Role { RoleName = "Customer" };
                await _context.Roles.AddAsync(customerRole);
                await _context.SaveChangesAsync();
            }
            return customerRole;
        }

    }
}
