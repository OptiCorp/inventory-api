namespace Inventory.Models.DocumentDTOs;

public class DocumentUploadDto
{
    public string? DocumentTypeId { get; set; }
    public IFormFile? File { get; set; }
}