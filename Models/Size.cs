using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Models
{
    public class Size
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        
        public string? ItemTemplateId { get; set; }
        
        public string? DocumentId { get; set; }
        
        public ItemTemplate? ItemTemplate { get; set; }
        
        
    }
}