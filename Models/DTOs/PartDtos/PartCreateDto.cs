using System.ComponentModel.DataAnnotations;

namespace Inventory.Models.DTO
{
    public class PartCreateDto
    {
        [Required]
        public string? WPId { get; set; }

        [Required]
        public string? SerialNumber { get; set; }

        [Required]
        public string? ProductNumber { get; set; }

        public string? Location { get; set; }

        [Required]
        public string? Description { get; set; }

        public string? ParentSubassemblyId { get; set; }

        [Required]
        public string? Vendor { get; set; }

        [Required]
        public string? AddedById { get; set; }

        public string? Comment { get; set; }
    }
}
