namespace BiblioTar.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        List<User> Users { get; set; }
    }
}
