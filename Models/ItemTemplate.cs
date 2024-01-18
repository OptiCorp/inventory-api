using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Models
{
    public class ItemTemplate
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        
        public string? Name { get; set; }
        
        public string? Type { get; set; }
        
        public string? CategoryId { get; set; }
        
        public string? ProductNumber { get; set; }
        
        public string? Revision { get; set; }
        
        public string? Description { get; set; }
        
        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
        
        public string? CreatedById { get; set; }
        
        public User? CreatedBy { get; set; }
        
        public Category? Category { get; set; }

        public IEnumerable<Document>? Documents { get; set; }
        
        public IEnumerable<Size>? Sizes { get; set; }
        
        public IEnumerable<Item>? Items { get; set; }
    }
}