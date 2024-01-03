using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Models
{
    public class Vendor
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        
        [Required]
        public string? Name { get; set; }
        
        [Required]
        public string? UserId { get; set; }
        
        [Required]
        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
        
        public User? User { get; set; }
    }
}