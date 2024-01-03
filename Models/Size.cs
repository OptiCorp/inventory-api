using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Models
{
    public class Size
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        
        [Required]
        public string? ItemTemplateId { get; set; }
        
        [Required]
        public string? Property { get; set; }
        
        [Required]
        public float? Amount { get; set; }
        
        [Required]
        public string? Unit { get; set; }
        
        public ItemTemplate? ItemTemplate { get; set; }
        
        
    }
}