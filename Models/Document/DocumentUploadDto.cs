namespace Inventory.Models.DTO;

public class DocumentUploadDto
{
    public string? DocumentTypeId { get; set; }
    public IFormFile? File { get; set; }
}