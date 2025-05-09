﻿using AutoMapper;
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
        Task<string> Delete(string id);
        Task<string> UpdateRole( UserUpdateDto userUpdateDto);
        Task<UserGetDto> GetUserById(string id);
        Task<List<UserGetDto>> GetAllUsers();
        Task<string> UpdateInformations(UserDtoToUpdateFunc userToUpdate);

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

        public async Task<string> Delete(string id)
        { 
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == id) ?? throw new Exception("User not found");
            user.IsEnabled = false;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return "User deleted successfully";
        }

        public async Task<string> UpdateRole(UserUpdateDto userUpdateDto)
        {
            
            if (!Enum.IsDefined(typeof(User.RoleEnums), userUpdateDto.Roles))
            {
                throw new Exception("Roles cannot be null");
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userUpdateDto.UserId.ToString()) ?? throw new Exception("User not found");
            user.Roles = userUpdateDto.Roles;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return "Role updated successfully";
            
        }

        public async Task<UserGetDto> GetUserById(string id)
        {
            UserGetDto userGetDto = new UserGetDto();
            var user = await _context.Users
                .Include(u => u.Address)
                .Include(x=>x.Borrows)
                .Include(c=>c.Reservations)
                .FirstOrDefaultAsync(u => u.Id.ToString() == id) ?? throw new Exception("User not found");
            
            return _mapper.Map<UserGetDto>(user);
        }

        public async Task<List<UserGetDto>> GetAllUsers()
        {
            var users = await _context.Users
                .Include(u => u.Address)
                .Include(x => x.Borrows)
                .Include(c => c.Reservations)
                .ToListAsync() ?? throw new Exception("No users found");
            
            return _mapper.Map<List<UserGetDto>>(users).ToList();
        }

        public async Task<string> UpdateInformations(UserDtoToUpdateFunc userToUpdate)
        {
            var user = await _context.Users
                .Include(c=>c.Address)
                .FirstOrDefaultAsync(v=> v.Id.ToString() == userToUpdate.Id) 
                       ?? throw new Exception("User not found"); 
            
                if (!string.IsNullOrEmpty(userToUpdate.PhoneNumber))
                {
                    user.PhoneNumber = userToUpdate.PhoneNumber;
                }
                
                if (!string.IsNullOrEmpty(userToUpdate.Address.ZipCode))
                {
                    user.Address.ZipCode = userToUpdate.Address.ZipCode;
                }
                
                if (!string.IsNullOrEmpty(userToUpdate.Address.City))
                {
                    user.Address.City = userToUpdate.Address.City;
                }
                
                if (!string.IsNullOrEmpty(userToUpdate.Address.Street))
                {
                    user.Address.Street = userToUpdate.Address.Street;
                }
                
                if (!string.IsNullOrEmpty(userToUpdate.Address.HouseNumber))
                {
                    user.Address.HouseNumber = userToUpdate.Address.HouseNumber;
                }
                
                if (!string.IsNullOrEmpty(userToUpdate.Address.Country))
                {
                    user.Address.Country = userToUpdate.Address.Country;
                }
            
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
           return "User updated successfully";
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
