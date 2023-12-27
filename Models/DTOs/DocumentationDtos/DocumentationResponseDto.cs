namespace Inventory.Models.DTOs.DocumentationDtos
{
    public class DocumentationResponseDto
    {
        public string? Id { get; set; }
        public string? BlobRef { get; set; }
        public string? ContentType { get; set; }
        public byte[]? Bytes { get; set; }
    }
}

