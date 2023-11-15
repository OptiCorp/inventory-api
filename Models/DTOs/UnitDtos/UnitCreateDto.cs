using System.ComponentModel.DataAnnotations;

namespace Inventory.Models.DTO
{
    public class UnitCreateDto
    {
        public string? WPId { get; set; }

        public string? SerialNumber { get; set; }

        public string? ProductNumber { get; set; }

        public string? Location { get; set; }

        public string? Description { get; set; }

        public string? Vendor { get; set; }

        public string? AddedById { get; set; }

        public string? Comment { get; set; }
    }
}