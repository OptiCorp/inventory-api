using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Models
{
    public class PreCheck
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        
        public string? ItemId { get; set; }
        
        public bool? Check { get; set; }
        
        public string? Comment { get; set; }
        
        public Item? Item { get; set; }
        
    }
}