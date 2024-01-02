using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Models
{
    public class Documentation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        
        [Required]
        public string? Name { get; set; }
        
        [Required]
        public string? ItemId { get; set; }
        
        [Required]
        public string? BlobRef { get; set; }
        
        [Required]
        public string? ContentType { get; set; }
        
        public Item? Item { get; set; }
    }
}

