using AutoMapper;
using BiblioTar.Context;
using BiblioTar.Controllers;
using BiblioTar.DTOs;
using BiblioTar.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BiblioTar.Service
{


    public interface IUserService
    {
        Task<int> RegisterCustomer(UserCreateDto userCreateDto);
        Task<int> RegisterEmployee(EmployeeCreateDto employeeCreateDto);
        Task<User?> Authenticate(string email, string password);
        Task<string> GenerateToken(User user);
    }


    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public UserService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<int> RegisterCustomer(UserCreateDto userCreateDto)
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
        public async Task<int> RegisterEmployee(EmployeeCreateDto employeeCreateDto)
        {
            var user = _mapper.Map<User>(employeeCreateDto);

            var address = new Address
            {
                ZipCode = employeeCreateDto.ZipCode,
                City = employeeCreateDto.City,
                Street = employeeCreateDto.Street,
                HouseNumber = employeeCreateDto.HouseNumber,
                Country = employeeCreateDto.Country
            };
            var temp = await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();//Ha midnen igaz, menteni kell, hogy az id-t megkapja
            user.AddressId = temp.Entity.Id;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }

        public async Task<User?> Authenticate(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
            return user;
        }

        



        private async Task<string> GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["kulcs"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(5));

            var id = await GetClaimsIdentity(user);
            var token = new JwtSecurityToken("https://localhost:7017", "https://localhost:7017", id.Claims, expires: expires, signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name), 
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Sid, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.Now.ToString(CultureInfo.InvariantCulture))
            };

            var roles = Enum.GetValues(typeof(User.RoleEnums))
                    .Cast<User.RoleEnums>()
                    .Where(r => user.Roles.HasFlag(r)); 

            foreach (var role in roles)
            {
                claims.Add(new Claim("roleIds", ((int)role).ToString()));
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }


            return new ClaimsIdentity(claims, "Token");
        }

    }
}
