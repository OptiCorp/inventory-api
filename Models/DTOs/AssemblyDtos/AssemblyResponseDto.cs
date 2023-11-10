using System.ComponentModel.DataAnnotations;

namespace Inventory.Models.DTO
{
    public class AssemblyResponseDto
    {
        public string? Id { get; set; }

        public string? WPId { get; set; }

        public string? SerialNumber { get; set; }

        public string? ProductNumber { get; set; }

        public string? Location { get; set; }

        public string? Description { get; set; }

        public string? UnitId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
