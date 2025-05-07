using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BiblioTar.Entities;


namespace BiblioTar.DTOs
{
    public class UserCreateDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } 
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string Country { get; set; }
    }

    public class UserInputDto
    {
        public int Id { get; set; }
    }
    
    public class EmployeeCreateDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        public int Roles { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string Country { get; set; }
    }
    
    public class UserGetDto
    {
        public int Id { get; set; } 
        public string Email { get; set; } 
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public DateTime RegistrationDate { get; set; } 
        public bool IsEnabled { get; set; } = true; //true == engedélyezett, false == letiltott
        public AddressGetDto Address { get; set; } 
        
        public User.RoleEnums Roles { get; set; } 
        public List<ReservationDto> Reservations { get; set; } 
        public List<BorrowDto> Borrows { get; set; }
    }
    
    public class UserUpdateDto
    {
        public int UserId { get; set; } 
        public User.RoleEnums Roles { get; set; } 
    }
    
    
    public class UserUpdateInformationDto
    {
        public string PhoneNumber { get; set; }
        public AddressCreateDto Address { get; set; }
    }
    
    public class UserDtoToUpdateFunc
    {
        public string Id { get; set; }
        public string PhoneNumber { get; set; }
        public AddressCreateDto Address { get; set; }
    }
    
}
