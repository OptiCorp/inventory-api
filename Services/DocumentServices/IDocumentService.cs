using Inventory.Models;
using Inventory.Models.DocumentDtos;

namespace Inventory.Services
{
    public interface IDocumentService
    {
        Task<IEnumerable<DocumentResponseDto>> GetDocumentsByItemIdAsync(string id);
        Task<DocumentResponseDto> GetDocumentByIdAsync(string id);
        Task<string> UploadDocumentAsync(DocumentUploadDto documentation);
        Task AddDocumentToItemAsync(string documentId, string itemId);
        Task AddDocumentToItemTemplateAsync(string documentId, string itemTemplateId);
        Task RemoveDocumentFromItemAsync(string documentId, string itemId);
        Task RemoveDocumentFromItemTemplateAsync(string documentId, string ItemTemplateId);
        Task DeleteDocumentAsync(string documentId);
    }
}

