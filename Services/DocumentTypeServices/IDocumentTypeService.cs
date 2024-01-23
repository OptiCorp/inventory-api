using Inventory.Models;
using Inventory.Models.DTO;

namespace Inventory.Services
{
    public interface IDocumentTypeService
    {
        Task<IEnumerable<DocumentType>> GetAllDocumentTypesAsync();
        Task<DocumentType?> GetDocumentTypeByIdAsync(string id);
        Task<string?> CreateDocumentTypeAsync(DocumentTypeCreateDto documentType);
        Task UpdateDocumentTypeAsync(DocumentType documentType);
        Task DeleteDocumentTypeAsync(string id);
    }
}