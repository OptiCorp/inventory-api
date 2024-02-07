namespace Inventory.Models.DTO;

public class ItemBusCreateDto
{
    public string? Id { get; set; }

    public string? WpId { get; set; }

    public string? SerialNumber { get; set; }

    public string? Category { get; set; }

    public string? ParentId { get; set; }
}