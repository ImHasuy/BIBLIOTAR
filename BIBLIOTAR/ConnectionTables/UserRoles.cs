using BiblioTar.Entities;

namespace BiblioTar.ConnectionTables
{
    public class UserRoles
    {
        
        public string UserEmail { get; set; }
        public int RoleId { get; set; }


        public User? User { get; set; }
        public Role? Role { get; set; }
    }
}
