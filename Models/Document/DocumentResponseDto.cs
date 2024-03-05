namespace Inventory.Models.DTO;

public class DocumentResponseDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? ContentType { get; set; }
    public byte[]? Bytes { get; set; }

    public string? FileName { get; set; }

}