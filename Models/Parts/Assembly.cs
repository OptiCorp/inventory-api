using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Inventory.Models
{
    public class Assembly
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }

        [Required]
        public string? WPId { get; set; }

        [Required]
        public string? SerialNumber { get; set; }

        [Required]
        public string? ProductNumber { get; set; }

        public string? DocumentationId { get; set; }

        public string? Location { get; set; }

        [Required]
        public string? Description { get; set; }

        public string? UnitId { get; set; }

        [Required]
        public string? Vendor { get; set; }

        [Required]
        public string? UserId { get; set; }

        public string? Comment { get; set; }

        [Required]
        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public Unit? Unit { get; }

        public ICollection<Subassembly>? Subassemblies { get; }

        public User? User { get; }
    }
}
