using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Models
{
    public class Size
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        
        public string? ItemTemplateId { get; set; }
        
        public string? Property { get; set; }
        
        public float? Amount { get; set; }
        
        public string? Unit { get; set; }
    }
}