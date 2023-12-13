using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Models
{
    public class Item
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }

        [Required]
        public string? WpId { get; set; }

        [Required]
        public string? SerialNumber { get; set; }

        [Required]
        public string? ProductNumber { get; set; }
        
        public string? Type { get; set; }
        
        public string? CategoryId { get; set; }
        
        public string? ParentId { get; set; }
        
        public string? VendorId { get; set; }
        
        public string? LocationId { get; set; }

        public string? DocumentationId { get; set; }
        
        public string? UserId { get; set; }

        [Required]
        public string? Description { get; set; }

        public string? Comment { get; set; }
        
        public string? ListId { get; set; }

        [Required]
        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
        
        public Category? Category { get; set; }
        
        public Item? Parent { get; set; }
        
        public IEnumerable<Item>? Children { get; set; }
        
        public Vendor? Vendor { get; set; }
        
        public Location? Location { get; set; }
        
        public IEnumerable<LogEntry>? LogEntries { get; set; }

        public User? User { get; set; }
        
    }
}