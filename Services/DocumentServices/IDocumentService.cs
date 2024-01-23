using Inventory.Models;
using Inventory.Models.DocumentDtos;

namespace Inventory.Services
{
    public interface IDocumentService
    {
        Task<IEnumerable<DocumentResponseDto>> GetDocumentsByItemIdAsync(string id);
        Task<DocumentResponseDto> GetDocumentByIdAsync(string id);
        Task<string?> AddDocumentToItemAsync(DocumentUploadDto document, string itemId);
        Task<string?> AddDocumentToItemTemplateAsync(DocumentUploadDto document, string itemTemplateId);
        Task DeleteDocumentAsync(string documentId);
    }
}

