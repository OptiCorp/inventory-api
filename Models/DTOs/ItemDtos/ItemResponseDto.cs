using System.ComponentModel.DataAnnotations;

namespace Inventory.Models.DTOs.ItemDtos
{
    public class ItemResponseDto
    {
        [Required]
        public string? Id { get; set; }

        [Required]
        public string? WpId { get; set; }

        [Required]
        public string? SerialNumber { get; set; }

        [Required]
        public string? ProductNumber { get; set; }
        
        [Required]
        public string? Type { get; set; }

        public string? Location { get; set; }

        [Required]
        public string? Description { get; set; }
        
        public string? ParentId { get; set; }

        [Required]
        public string? Vendor { get; set; }
        
        public string? AddedById { get; set; }

        public string? Comment { get; set; }
        
        public string? ListId { get; set; }
        
        public ItemResponseDto? Parent { get; set; }
        
        public IEnumerable<ItemResponseDto>? Children { get; set; }

        [Required]
        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}