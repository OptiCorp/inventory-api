using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Models
{
    public class ItemTemplate
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        
        [Required]
        public string? Name { get; set; }
        
        [Required]
        public string? Type { get; set; }
        
        [Required]
        public string? ProductNumber { get; set; }
        
        public string? Revision { get; set; }
        
        [Required]
        public string? Description { get; set; }
        
        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
        
        [Required]
        public string? CreatorId { get; set; }
        
        public User? Creator { get; set; }

        public IEnumerable<Document>? Documents { get; set; }
        
        public IEnumerable<Size>? Sizes { get; set; }
        
        public IEnumerable<Item>? Items { get; set; }
    }
}