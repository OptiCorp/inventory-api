using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Models
{
    public class List
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }

        public string? Title { get; set; }

        public string? CreatedById { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public IEnumerable<Item>? Items { get; set; }

        public User? CreatedBy { get; set; }
    }
}