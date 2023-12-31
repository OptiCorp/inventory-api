using System.ComponentModel.DataAnnotations;

namespace Inventory.Models.DTOs.ListDTOs;

public class ListUpdateDto
{
    [Required]
    public string? Id { get; set; }
    
    [Required]
    public string? Title { get; set; }
}