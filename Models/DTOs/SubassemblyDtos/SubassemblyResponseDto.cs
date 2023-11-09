using System.ComponentModel.DataAnnotations;

namespace Inventory.Models.DTO
{
    public class SubassemblyResponseDto
    {
        public string? Id { get; set; }

        public string? WPId { get; set; }

        public string? SerialNumber { get; set; }

        public string? ProductNumber { get; set; }

        public string? Location { get; set; }

        public string? Description { get; set; }

        public AssemblyResponseDto? Unit { get; set; }

        public ICollection<SubassemblyResponseDto>? Subassemblies { get; set; }

        public ICollection<ItemResponseDto>? Items { get; set; }
    }
}
