namespace Inventory.Models.DocumentDtos;

public class DocumentUploadDto
{
    public string? ItemId { get; set; }
    public string? DocumentTypeId { get; set; }
    public IFormFile? File { get; set; }
}