using System.ComponentModel.DataAnnotations;

namespace Inventory.Models.DTO
{
    public class ItemCreateDto
    {
        public string? WPId { get; set; }

        public string? SerialNumber { get; set; }

        public string? ProductNumber { get; set; }

        public string? Location { get; set; }

        public string? Description { get; set; }

        public string? SubassemblyId { get; set; }
    }
}
