using Azure.Identity;
using Azure.Storage.Blobs;
using Inventory.Models;
using Inventory.Models.DocumentDtos;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly InventoryDbContext _context;

        public DocumentService(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DocumentResponseDto>> GetDocumentsByItemIdAsync(string id)
        {
            try
            {
                var item = await _context.Items
                    .Where(item => item.Id == id)
                    .Include(item => item.Documents)
                    .FirstOrDefaultAsync();

                var itemTemplate = await _context.ItemTemplates
                    .Where(temp => temp.Id == item!.ItemTemplateId)
                    .Include(doc => doc.Documents)
                    .FirstOrDefaultAsync();

                var allDocuments = new List<Document>();
                if (item?.Documents != null) allDocuments.AddRange(item.Documents);
                if (itemTemplate?.Documents != null)
                    allDocuments.AddRange(itemTemplate.Documents);

                if (!allDocuments.Any()) return new List<DocumentResponseDto>();

                List<DocumentResponseDto> documentResponseList = new List<DocumentResponseDto>();

                foreach (var document in allDocuments)
                {
                    var fileBytes = await DownloadDocumentFromBlobStorage(document.BlobId!);

                    var documentResponse = new DocumentResponseDto
                    {
                        Id = document.Id,
                        Name = await _context.DocumentTypes.Where(doc => doc.Id == document.DocumentTypeId)
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

        public async Task<DocumentResponseDto> GetDocumentByIdAsync(string id)
        {
            try
            {
                var document = await _context.Documents
                    .Where(doc => doc.Id == id)
                    .Include(doc => doc.DocumentType)
                    .FirstOrDefaultAsync();

                var fileBytes = await DownloadDocumentFromBlobStorage(document!.BlobId!);

                var documentResponse = new DocumentResponseDto
                {
                    Id = document.Id,
                    Name = document.DocumentType!.Name,
                    ContentType = document.ContentType,
                    Bytes = fileBytes
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
                var blobContainerEndpoint = Environment.GetEnvironmentVariable("blobContainerEndpoint");
                BlobContainerClient blobContainerClient =
                    new BlobContainerClient(new Uri(blobContainerEndpoint!), new DefaultAzureCredential());

                var blobId = Guid.NewGuid().ToString();
                var blobExists = await blobContainerClient.GetBlobClient(blobId).ExistsAsync();

                while (blobExists == true)
                {
                    blobId = Guid.NewGuid().ToString();
                    blobExists = await blobContainerClient.GetBlobClient(blobId).ExistsAsync();
                }

                if (document.File != null)
                {
                    var newDocument = new Document
                    {
                        DocumentTypeId = document.DocumentTypeId,
                        BlobId = blobId,
                        ContentType = document.File.ContentType,
                        ItemId = itemId
                    };

                    await blobContainerClient.CreateIfNotExistsAsync();

                    using (MemoryStream documentStream = new())
                    {
                        if (document.File != null) await document.File.CopyToAsync(documentStream);
                        documentStream.Position = 0;
                        await blobContainerClient.UploadBlobAsync(newDocument.BlobId, documentStream);
                    }

                    await _context.Documents.AddAsync(newDocument);
                    await _context.SaveChangesAsync();

                    return newDocument.Id;
                }

                return null;
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
                var blobContainerEndpoint = Environment.GetEnvironmentVariable("blobContainerEndpoint");
                BlobContainerClient blobContainerClient =
                    new BlobContainerClient(new Uri(blobContainerEndpoint!), new DefaultAzureCredential());

                var blobId = Guid.NewGuid().ToString();
                var blobExists = await blobContainerClient.GetBlobClient(blobId).ExistsAsync();

                while (blobExists == true)
                {
                    blobId = Guid.NewGuid().ToString();
                    blobExists = await blobContainerClient.GetBlobClient(blobId).ExistsAsync();
                }

                if (document.File != null)
                {
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

                    await _context.Documents.AddAsync(newDocument);
                    await _context.SaveChangesAsync();

                    return newDocument.Id;
                }

                return null;
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
                var document = await _context.Documents.FirstOrDefaultAsync(doc => doc.Id == documentId);

                var blobContainerEndpoint = Environment.GetEnvironmentVariable("blobContainerEndpoint");
                BlobContainerClient blobContainerClient =
                    new BlobContainerClient(new Uri(blobContainerEndpoint!), new DefaultAzureCredential());

                var blobClient = blobContainerClient.GetBlobClient(document!.BlobId);

                var response = await blobClient.DeleteAsync();

                if (response.Status != StatusCodes.Status200OK) return;

                _context.Documents.Remove(document);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


        }

        private static async Task<byte[]> DownloadDocumentFromBlobStorage(string blobId)
        {
            try
            {
                var blobContainerEndpoint = Environment.GetEnvironmentVariable("blobContainerEndpoint");
                BlobContainerClient blobContainerClient =
                    new BlobContainerClient(new Uri(blobContainerEndpoint!), new DefaultAzureCredential());

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
}