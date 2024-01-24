using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Models
{
    public class DocumentType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}