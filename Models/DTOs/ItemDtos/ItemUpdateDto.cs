using System.ComponentModel.DataAnnotations;
namespace Inventory.Models.DTOs.ItemDTOs
{
    public class ItemUpdateDto
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
        
        [Required]
        public string? Description { get; set; }
        
        public string? Comment { get; set; }
        
        public string? ListId { get; set; }
    }
}