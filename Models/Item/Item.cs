using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Models
{
    public class Item
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        
        [MaxLength(100)]
        public string? WpId { get; set; }
        
        [MaxLength(100)]
        public string? SerialNumber { get; set; }
        
        [MaxLength(100)]
        public string? ParentId { get; set; }
        
        [MaxLength(100)]
        public string? VendorId { get; set; }
        
        [MaxLength(100)]
        public string? LocationId { get; set; }
        
        [MaxLength(100)]
        public string? CreatedById { get; set; }
        
        [MaxLength(1000)]
        public string? Comment { get; set; }
        
        [MaxLength(100)]
        public string? ItemTemplateId { get; set; }
        
        [MaxLength(100)]
        public string? PreCheckId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public ICollection<Item>? Children { get; set; }

        public Vendor? Vendor { get; set; }

        public Location? Location { get; set; }

        public IEnumerable<LogEntry>? LogEntries { get; set; }

        public User? CreatedBy { get; set; }

        public PreCheck? PreCheck { get; set; }

        public ItemTemplate? ItemTemplate { get; set; }

        public ICollection<Document>? Documents { get; set; }

        public Item? Parent { get; set; }
    }
}