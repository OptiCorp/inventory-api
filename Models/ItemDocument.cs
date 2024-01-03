using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Models
{
    public class ItemDocument
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        
        [Required]
        public string? ItemId { get; set; }
        
        [Required]
        public string? DocumentId { get; set; }
        
        public Item? Item { get; set; }
        
        public Document? Document { get; set; }
        
    }
}