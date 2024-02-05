
namespace Inventory.Models.DTO;

public class DocumentTypeCreateDto(string? name, string? description)
{
    public string? Name { get; } = name;

    public string? Description { get; } = description;
}