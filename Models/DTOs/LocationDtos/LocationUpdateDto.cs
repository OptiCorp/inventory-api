using System.ComponentModel.DataAnnotations;

namespace Inventory.Models.DTOs.LocationDTOs
{
    public class LocationUpdateDto
    {
        [Required]
        public string? Id { get; set; }
        
        [Required]
        public string? Name { get; set; }
    }
}