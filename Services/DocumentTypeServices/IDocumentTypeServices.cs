using Inventory.Models;

namespace Inventory.Services
{
    public interface IDocumentTypeService
    {
        Task<IEnumerable<DocumentType>> GetAllDocumentTypesAsync();
        Task<DocumentType> GetDocumentTypeByIdAsync(string id);
        Task<string?> CreateDocumentTypeAsync(DocumentType documentType);
        Task UpdateDocumentTypeAsync(DocumentType documentType);
        Task DeleteDocumentTypeAsync(string id);
    }
}