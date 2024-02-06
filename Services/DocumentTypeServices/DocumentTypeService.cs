using Inventory.Models;
using Inventory.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Services;

public class DocumentTypeService(InventoryDbContext context) : IDocumentTypeService
{
    public async Task<IEnumerable<DocumentType>> GetAllDocumentTypesAsync()
    {
        try
        {
            return await context.DocumentTypes
                .OrderBy(c => c.Name == "other" ? int.MaxValue : (int?)null)
                .ThenBy(c => c.Name)
                .ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<DocumentType?> GetDocumentTypeByIdAsync(string id)
    {
        try
        {
            return await context.DocumentTypes.FirstOrDefaultAsync(c => c.Id == id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<string?> CreateDocumentTypeAsync(DocumentTypeCreateDto documentTypeCreate)
    {
        try
        {
            var documentType = new DocumentType
            {
                Name = documentTypeCreate.Name,
                Description = documentTypeCreate.Description

            };

            await context.DocumentTypes.AddAsync(documentType);
            await context.SaveChangesAsync();
            return documentType.Id;
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
            var documentType = await context.DocumentTypes.FirstOrDefaultAsync(c => c.Id == documentTypeUpdate.Id);

            if (documentType != null)
            {
                documentType.Name = documentTypeUpdate.Name;
                documentType.Description = documentTypeUpdate.Description;

                await context.SaveChangesAsync();
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
            var documentType = await context.DocumentTypes.FirstOrDefaultAsync(c => c.Id == id);
            if (documentType != null)
            {
                context.DocumentTypes.Remove(documentType);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}