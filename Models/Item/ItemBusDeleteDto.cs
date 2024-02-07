namespace Inventory.Models.DTO;

public class ItemBusDeleteDto
{
    public string? Id { get; set; }

    public bool? DeleteSubItems { get; set; }
}