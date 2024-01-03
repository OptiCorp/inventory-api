using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Models
{
    public class ItemTemplateDocument
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        
        [Required]
        public string? ItemTemplateId { get; set; }
        
        [Required]
        public string? DocumentId { get; set; }
        
        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
        
        public ItemTemplate? ItemTemplate { get; set; }
        
        public Document? Document { get; set; }
        
    }
}