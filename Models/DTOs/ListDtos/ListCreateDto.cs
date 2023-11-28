using System.ComponentModel.DataAnnotations;

namespace Inventory.Models.DTOs.ListDtos;

public class ListCreateDto
{
    [Required]
    public string? Title { get; set; }
    
    [Required]
    public string? CreatedById { get; set; }
}