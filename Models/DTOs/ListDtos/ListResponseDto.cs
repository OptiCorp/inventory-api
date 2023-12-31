using System.ComponentModel.DataAnnotations;
using Inventory.Models.DTO;
using Inventory.Models.DTOs.ItemDTOs;

namespace Inventory.Models.DTOs.ListDTOs;

public class ListResponseDto
{   
    [Required]
    public string? Id { get; set; }
    
    [Required]
    public string? Title { get; set; }
    
    [Required]
    public string? CreatedById { get; set; }
    
    [Required]
    public string? CreatedDate { get; set; }
    
    public string? UpdatedDate { get; set; }
    
    public IEnumerable<ItemResponseDto>? Items { get; set; }
    
    public UserDto? User { get; set; }
}