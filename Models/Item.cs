using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Inventory.Models
{
    public class Item
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }

        [Required]
        public string? WpId { get; set; }

        [Required]
        public string? SerialNumber { get; set; }

        [Required]
        public string? ProductNumber { get; set; }
        
        public string? Type { get; set; }

        public string? DocumentationId { get; set; }

        public string? Location { get; set; }

        [Required]
        public string? Description { get; set; }

        public string? ParentId { get; set; }

        [Required]
        public string? Vendor { get; set; }

        public string? UserId { get; set; }

        public string? Comment { get; set; }

        [Required]
        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
        
        public Item? ParentItem { get; }

        public User? User { get; }
    }
}