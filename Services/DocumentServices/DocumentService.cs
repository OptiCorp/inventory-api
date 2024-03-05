using Azure.Identity;
using Azure.Storage.Blobs;
using Inventory.Models;
using Inventory.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Services;

public class DocumentService(InventoryDbContext context) : IDocumentService
{
    private const string BlobContainerEndpoint =
        "https://storageaccountinventory.blob.core.windows.net/item-documentation";

    public async Task<IEnumerable<DocumentResponseDto>> GetDocumentsByItemIdAsync(string id)
    {
        try
        {
            var item = await context.Items
                .Where(item => item.Id == id)
                .Include(item => item.Documents)
                .FirstOrDefaultAsync();

            var itemTemplate = await context.ItemTemplates
                .Where(temp => item != null && temp.Id == item.ItemTemplateId)
                .Include(doc => doc.Documents)
                .FirstOrDefaultAsync();

            var allDocuments = new List<Document>();
            if (item?.Documents != null) allDocuments.AddRange(item.Documents);
            if (itemTemplate?.Documents != null)
                allDocuments.AddRange(itemTemplate.Documents);

            if (allDocuments.Count == 0) return new List<DocumentResponseDto>();

            var documentResponseList = new List<DocumentResponseDto>();

            foreach (var document in allDocuments)
            {
                if (document.BlobId == null) continue;
                var fileBytes = await DownloadDocumentFromBlobStorage(document.BlobId);

                var documentResponse = new DocumentResponseDto
                {
                    Id = document.Id,
                    Name = await context.DocumentTypes.Where(doc => doc.Id == document.DocumentTypeId)
                        .Select(doc => doc.Name).FirstOrDefaultAsync(),
                    ContentType = document.ContentType,
                    Bytes = fileBytes
                };

                documentResponseList.Add(documentResponse);
            }

            return documentResponseList;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<DocumentResponseDto?> GetDocumentByIdAsync(string id)
    {
        try
        {
            var document = await context.Documents
                .Where(doc => doc.Id == id)
                .Include(doc => doc.DocumentType)
                .FirstOrDefaultAsync();

            if (document?.BlobId == null) return null;
            var fileBytes = await DownloadDocumentFromBlobStorage(document.BlobId);

            var documentResponse = new DocumentResponseDto
            {
                Id = document.Id,
                Name = document.DocumentType?.Name,
                ContentType = document.ContentType,
                Bytes = fileBytes,
                FileName = document.FileName,
            };

            return documentResponse;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<string?> AddDocumentToItemAsync(DocumentUploadDto document, string itemId)
    {
        try
        {
            var blobContainerClient =
                new BlobContainerClient(new Uri(BlobContainerEndpoint), new DefaultAzureCredential());

            var blobId = Guid.NewGuid().ToString();
            var blobExists = await blobContainerClient.GetBlobClient(blobId).ExistsAsync();

            while (blobExists == true)
            {
                blobId = Guid.NewGuid().ToString();
                blobExists = await blobContainerClient.GetBlobClient(blobId).ExistsAsync();
            }

            

            if (document.File == null) return null;
            var newDocument = new Document
            {
                DocumentTypeId = document.DocumentTypeId,
                BlobId = blobId,
                ContentType = document.File.ContentType,
                ItemId = itemId,
                FileName = document.File.FileName,
            };

            await blobContainerClient.CreateIfNotExistsAsync();

            using (MemoryStream documentStream = new())
            {
                if (document.File != null) await document.File.CopyToAsync(documentStream);
                documentStream.Position = 0;
                await blobContainerClient.UploadBlobAsync(newDocument.BlobId, documentStream);
            }

            await context.Documents.AddAsync(newDocument);
            await context.SaveChangesAsync();

            return newDocument.Id;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<string?> AddDocumentToItemTemplateAsync(DocumentUploadDto document, string itemTemplateId)
    {
        try
        {
            var blobContainerClient =
                new BlobContainerClient(new Uri(BlobContainerEndpoint), new DefaultAzureCredential());

            var blobId = Guid.NewGuid().ToString();
            var blobExists = await blobContainerClient.GetBlobClient(blobId).ExistsAsync();

            while (blobExists == true)
            {
                blobId = Guid.NewGuid().ToString();
                blobExists = await blobContainerClient.GetBlobClient(blobId).ExistsAsync();
            }

            if (document.File == null) return null;
            var newDocument = new Document
            {
                DocumentTypeId = document.DocumentTypeId,
                BlobId = blobId,
                ContentType = document.File.ContentType,
                ItemTemplateId = itemTemplateId
            };

            await blobContainerClient.CreateIfNotExistsAsync();

            using (MemoryStream documentStream = new())
            {
                await document.File.CopyToAsync(documentStream);
                documentStream.Position = 0;
                await blobContainerClient.UploadBlobAsync(newDocument.BlobId, documentStream);
            }

            await context.Documents.AddAsync(newDocument);
            await context.SaveChangesAsync();

            return newDocument.Id;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    public async Task DeleteDocumentAsync(string documentId)
    {
        try
        {
            var document = await context.Documents.FirstOrDefaultAsync(doc => doc.Id == documentId);

            var blobContainerClient =
                new BlobContainerClient(new Uri(BlobContainerEndpoint), new DefaultAzureCredential());

            var blobClient = blobContainerClient.GetBlobClient(document?.BlobId);

            var response = await blobClient.DeleteAsync();

            if (response.Status != 202) return;

            if (document != null) context.Documents.Remove(document);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }


    }

    private static async Task<byte[]?> DownloadDocumentFromBlobStorage(string blobId)
    {
        try
        {
            var blobContainerClient =
                new BlobContainerClient(new Uri(BlobContainerEndpoint), new DefaultAzureCredential());

            using MemoryStream documentStream = new();

            var blobClient = blobContainerClient.GetBlobClient(blobId);

            await blobClient.DownloadToAsync(documentStream);

            return documentStream.ToArray();

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}