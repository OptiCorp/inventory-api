using System.ComponentModel.DataAnnotations;
using Inventory.Models.DTO;

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
        
        public string? CategoryId { get; set; }
        
        public string? ParentId { get; set; }
        
        public string? VendorId { get; set; }

        public string? LocationId { get; set; }
        
        public string? AddedById { get; set; }
        public string? AddedByFirstName { get; set; } 
        public string? AddedByLastName { get; set; } 
        
        [Required]
        public string? Description { get; set; }
        
        public string? Comment { get; set; }
        
        public string? ListId { get; set; }

        [Required]
        public string? CreatedDate { get; set; }

        public string? UpdatedDate { get; set; }
        
        public Category? Category { get; set; }
        
        public ItemResponseDto? Parent { get; set; }
        
        public IEnumerable<ItemResponseDto>? Children { get; set; }
        
        public Vendor? Vendor { get; set; }
        
        public Location? Location { get; set; }
        
        public UserDto? User { get; set; }
    }
}