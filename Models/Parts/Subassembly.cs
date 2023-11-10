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

        public Assembly? Assembly { get; }

        public string? SubassemblyId { get; set; }

        public ICollection<Subassembly>? Subassemblies { get; }

        public ICollection<Item>? Items { get; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
