using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Models
{
    public class UserRole
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [StringLength(50)]
        public string? Id { get; set; }

        [StringLength(150)]
        public string? Name { get; set; }
        
        public IEnumerable<User>? Users { get; set; }
    }
}