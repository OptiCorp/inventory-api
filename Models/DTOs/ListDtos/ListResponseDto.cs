using System.Collections;
using System.ComponentModel.DataAnnotations;
using Inventory.Models.DTOs.ItemDtos;

namespace Inventory.Models.DTOs.ListDtos;

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
}