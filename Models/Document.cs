using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Models
{
    public class Document
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        
        public string? DocumentTypeId { get; set; }
        
        public string? BlobId { get; set; }
        
        public string? ContentType { get; set; }
        
        public DocumentType? DocumentType { get; set; }
        
        public IEnumerable<ItemTemplate>? ItemTemplates { get; set; }
        
        public IEnumerable<Item>? Items { get; set; }
    }
}