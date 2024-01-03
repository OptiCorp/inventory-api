using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Models
{
    public class PartDocument
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        
        public string? ItemId { get; set; }
        
        public string? DocumentId { get; set; }
        
        public Item? Item { get; set; }
        
        public Document? Document { get; set; }
        
    }
}