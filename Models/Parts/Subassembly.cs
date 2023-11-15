using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Inventory.Models
{
    public class Subassembly
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }

        public string? WPId { get; set; }

        public string? SerialNumber { get; set; }

        public string? ProductNumber { get; set; }

        public string? DocumentationId { get; set; }

        public string? Location { get; set; }

        public string? Description { get; set; }

        public string? AssemblyId { get; set; }

        public string? SubassemblyId { get; set; }

        public string? Vendor { get; set; }

        public string? UserId { get; set; }

        public string? Comment { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public Assembly? Assembly { get; }

        public ICollection<Subassembly>? Subassemblies { get; }

        public ICollection<Item>? Items { get; }
    }
}
