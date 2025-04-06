using BiblioTar.ConnectionTables;

namespace BiblioTar.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public List<UserRoles> UserRoles { get; set; }
    }
}
