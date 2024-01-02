using Inventory.Models;
using Inventory.Models.DTOs.DocumentationDtos;

namespace Inventory.Utilities.DocumentationUtilities
{
    public class DocumentationUtilities : IDocumentationUtilities
    {
        public DocumentationResponseDto DocumentationToResponseDto(Documentation documentation, byte[] document)
        {
            var documentationResponseDto = new DocumentationResponseDto
            {
                Id = documentation.Id,
                BlobRef = documentation.BlobRef,
                Name = documentation.Name,
                ContentType = documentation.ContentType,
                Bytes = document
            };

            return documentationResponseDto;
        }
        
    }
}

