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
        Task<User> Authenticate(LoginDto logindto);
        Task<string> GenerateToken(User user);
        Task<string> Login(LoginDto loginDto);

    }


    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public UserService(AppDbContext context, IMapper mapper,IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }


        public async Task<string> Login(LoginDto loginDto)
        {
            var user = await Authenticate(loginDto);
            return await GenerateToken(user);
            
            
        }
        public async Task<int> RegisterCustomer(UserCreateDto userCreateDto)
        {
            var user = _mapper.Map<User>(userCreateDto);

            user.Password=BCrypt.Net.BCrypt.HashPassword(user.Password);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
    
            var address =  new Address
            { 
                ZipCode = userCreateDto.ZipCode,
                City = userCreateDto.City,
                Street = userCreateDto.Street,
                HouseNumber = userCreateDto.HouseNumber,
                Country = userCreateDto.Country,
                UserId = user.Id,
            };
            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();
            
            return user.Id;

        }
        public async Task<int> RegisterEmployee(EmployeeCreateDto employeeCreateDto)
        {
            var user = _mapper.Map<User>(employeeCreateDto);

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();//To generate an Id for the user
            
            var address = new Address
            {
                ZipCode = employeeCreateDto.ZipCode,
                City = employeeCreateDto.City,
                Street = employeeCreateDto.Street,
                HouseNumber = employeeCreateDto.HouseNumber,
                Country = employeeCreateDto.Country,
                UserId = user.Id,
            };
            
            var temp = await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();
            
            return user.Id;
        }

        public async Task<User> Authenticate(LoginDto logindto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == logindto.Email) ?? throw new Exception("User not found");
            bool isValid = BCrypt.Net.BCrypt.Verify(logindto.Password, user.Password);
            return isValid ? user : throw new Exception("Invalid password");
        }

        public async Task<string> GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtSettings:ExpiresInDays"]));

            var id = await GetClaimsIdentity(user);
            var token = new JwtSecurityToken(_configuration["JwtSettings:Issuer"], _configuration["JwtSettings:Audience"], id.Claims, expires: expires, signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ClaimsIdentity> GetClaimsIdentity(User user)
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
