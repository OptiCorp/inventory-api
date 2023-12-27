using Inventory.Models;
using Inventory.Models.DTOs.DocumentationDtos;

namespace Inventory.Utilities.DocumentationUtilities
{
    public interface IDocumentationUtilities
    {
        public DocumentationResponseDto DocumentationToResponseDto(Documentation documentation, byte[] document);
    }
}

