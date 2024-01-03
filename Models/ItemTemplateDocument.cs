using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Models
{
    public class ItemTemplateDocument
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        
        public string? ItemTemplateId { get; set; }
        
        public string? DocumentId { get; set; }
        
        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
        
        public ItemTemplate? ItemTemplate { get; set; }
        
        public Document? Document { get; set; }
        
    }
}