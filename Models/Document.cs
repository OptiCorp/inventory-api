using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Models
{
    public class Document
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        
        [Required]
        public string? DocumentTypeId { get; set; }
        
        [Required]
        public string? BlobId { get; set; }
        
        public DocumentType? DocumentType { get; set; }
        
        public IEnumerable<ItemTemplate>? ItemTemplates { get; set; }
        
        public IEnumerable<Item>? Items { get; set; }
    }
}