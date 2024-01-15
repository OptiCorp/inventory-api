using Inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Services
{
    public class DocumentTypeService : IDocumentTypeService
    {
        private readonly InventoryDbContext _context;

        public DocumentTypeService(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DocumentType>> GetAllDocumentTypesAsync()
        {
            return await _context.DocumentTypes.ToListAsync();
        }
        
        public async Task<DocumentType> GetDocumentTypeByIdAsync(string id)
        {
            return await _context.DocumentTypes.FirstOrDefaultAsync(c => c.Id == id);
        }
        
        public async Task<string?> CreateDocumentTypeAsync(DocumentType documentTypeCreate)
        {
            try
            {
                await _context.DocumentTypes.AddAsync(documentTypeCreate);
                await _context.SaveChangesAsync();
                return documentTypeCreate.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task UpdateDocumentTypeAsync(DocumentType documentTypeUpdate)
        {
            var documentType = await _context.DocumentTypes.FirstOrDefaultAsync(c => c.Id == documentTypeUpdate.Id);
        
            if (documentType != null)
            {
                documentType.Name = documentTypeUpdate.Name;
                documentType.Description = documentTypeUpdate.Description;
        
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteDocumentTypeAsync(string id)
        {
            var documentType = await _context.DocumentTypes.FirstOrDefaultAsync(c => c.Id == id);
            if (documentType != null)
            {
                _context.DocumentTypes.Remove(documentType);
                await _context.SaveChangesAsync();
            }
        }
    }
}
