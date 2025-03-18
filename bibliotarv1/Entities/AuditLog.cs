using System.ComponentModel.DataAnnotations;

namespace bibliotar.Entities
{
    public class AuditLog
    {
        [Key]
        public int LogId { get; set; }
        public int AdminId { get; set; }
        public string Action { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
