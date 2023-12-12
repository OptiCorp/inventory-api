using System.ComponentModel.DataAnnotations;

namespace Inventory.Models.DTOs.LocationDtos
{
    public class LocationCreateDto
    {
        [Required]
        public string? Name { get; set; }
        
        [Required]
        public string? AddedById { get; set; }
    }
}