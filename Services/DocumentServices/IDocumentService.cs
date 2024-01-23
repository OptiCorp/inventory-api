using Inventory.Models;
using Inventory.Models.DocumentDtos;

namespace Inventory.Services
{
    public interface IDocumentService
    {
        Task<IEnumerable<DocumentResponseDto>> GetDocumentsByItemIdAsync(string id);
        Task<Document?> GetDocumentByIdAsync(string id);
        Task<string?> UploadDocumentAsync(DocumentUploadDto documentation);

        Task DeleteDocumentFromItemAsync(Document document, string itemId);
    }
}

