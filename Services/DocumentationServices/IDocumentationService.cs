using Inventory.Controllers;
using Inventory.Models;
using Inventory.Models.DTOs.DocumentationDtos;

namespace Inventory.Services
{
    public interface IDocumentationService
    {
        Task<IEnumerable<DocumentationResponseDto>> GetDocumentationByItemId(string id);
        Task<Documentation> GetDocumentationById(string id);
        Task<string[]> UploadDocumentationAsync(DocumentationCreateDto documentation);

        Task DeleteDocumentFromItem(Documentation document);
    }
}

