using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Models
{
    public class LogEntry
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }

        [Required]
        public string? ItemId { get; set; }
        
        [Required]
        public string? UserId { get; set; }
        
        [Required]
        public string? Message { get; set; }
        
        [Required]
        public DateTime? CreatedDate { get; set; }

        public User? User { get; set; }
    }
}