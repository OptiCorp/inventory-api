using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Inventory.Models
{
    public class UserRole
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [StringLength(50)]
        public string? Id { get; set; }

        [StringLength(150)]
        public string? Name { get; set; }
    }
}