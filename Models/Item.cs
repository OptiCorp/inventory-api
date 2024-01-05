using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Models
{
    public class Item
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        
        public string? WpId { get; set; }
        
        public string? SerialNumber { get; set; }
        
        public string? CategoryId { get; set; }
        
        public string? ParentId { get; set; }
        
        public string? VendorId { get; set; }
        
        public string? LocationId { get; set; }
        
        public string? CreatedById { get; set; }

        public string? Comment { get; set; }
        
        public string? ListId { get; set; }
        
        public string? ItemTemplateId { get; set; }
        
        public string? PreCheckId { get; set; }
        
        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
        
        public Category? Category { get; set; }
        
        public Item? Parent { get; set; }
        
        public IEnumerable<Item>? Children { get; set; }
        
        public Vendor? Vendor { get; set; }
        
        public Location? Location { get; set; }
        
        public IEnumerable<LogEntry>? LogEntries { get; set; }

        public User? CreatedBy { get; set; }
        
        public PreCheck? PreCheck { get; set; }
        
        public ItemTemplate? ItemTemplate { get; set; }
        
        public IEnumerable<Document>? Documents { get; set; }
    }
}