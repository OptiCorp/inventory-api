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
    public DateTime? CreatedDate { get; set; }
    
    public DateTime? UpdatedDate { get; set; }
    
    public IEnumerable<ItemResponseDto>? Items { get; set; }
}