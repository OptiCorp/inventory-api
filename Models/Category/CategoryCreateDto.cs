
namespace Inventory.Models.DTO;

public class CategoryCreateDto(string? name, string? createdById)
{
    public string? Name { get; } = name;

    public string? CreatedById { get; } = createdById;
}