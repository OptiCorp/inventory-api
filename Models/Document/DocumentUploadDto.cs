namespace Inventory.Models.DocumentDTOs;

public class DocumentUploadDto(string? documentTypeId, IFormFile? file)
{
    public string? DocumentTypeId { get; } = documentTypeId;
    public IFormFile? File { get; } = file;
}