using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Models
{
    public class LogEntry
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }

        public string? ItemId { get; set; }
        
        public string? ItemTemplateId { get; set; }

        public string? CreatedById { get; set; }

        public string? Message { get; set; }

        public DateTime? CreatedDate { get; set; }

        public User? CreatedBy { get; set; }
    }
}