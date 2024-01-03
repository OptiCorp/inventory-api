using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Models
{
    public class ItemTemplate
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        
        public string? Name { get; set; }
        
        public string? Type { get; set; }
        
        public string? Revision { get; set; }
        
        public string? Description { get; set; }
        
        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
        
        public string? CreatorId { get; set; }
        
        public User? Creator { get; set; }

        
    }
}