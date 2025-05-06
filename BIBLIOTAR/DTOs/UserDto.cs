using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioTar.DTOs
{
    public class UserCreateDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } 
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

    public class UserGetByIdDto
    {
        public int Id { get; set; }
    }
    
    public class EmployeeCreateDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
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
}
