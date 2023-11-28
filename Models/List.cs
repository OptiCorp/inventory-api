using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Inventory.Models
{
    public class List
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        
        [Required]
        public string? Title { get; set; }
        
        [Required]
        public string? UserId { get; set; }
        
        [Required]
        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public IEnumerable<Item>? Items { get; set; }
        
        public User? User { get; set; }
    }
}