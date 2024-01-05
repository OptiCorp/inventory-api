using Inventory.Models;

namespace Inventory.Services
{
    public interface IDocumentService
    {
        Task<IEnumerable<Document>> GetDocumentsByItemIdAsync(string id);
        Task<Document> GetDocumentByIdAsync(string id);
        Task<string[]> UploadDocumentAsync(UploadDocumentDto documentation);

        Task DeleteDocumentFromItemAsync(Document document);
    }
}

