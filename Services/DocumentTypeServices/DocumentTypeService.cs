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
            try
            {
                return await _context.DocumentTypes.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task<DocumentType> GetDocumentTypeByIdAsync(string id)
        {
            try
            {
                return await _context.DocumentTypes.FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
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
            try
            {
                var documentType = await _context.DocumentTypes.FirstOrDefaultAsync(c => c.Id == documentTypeUpdate.Id);
        
                if (documentType != null)
                {
                    documentType.Name = documentTypeUpdate.Name;
                    documentType.Description = documentTypeUpdate.Description;
        
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task DeleteDocumentTypeAsync(string id)
        {
            try
            {
                var documentType = await _context.DocumentTypes.FirstOrDefaultAsync(c => c.Id == id);
                if (documentType != null)
                {
                    _context.DocumentTypes.Remove(documentType);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
