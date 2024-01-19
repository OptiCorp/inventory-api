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
                    .Where(temp => temp.Id == item.ItemTemplateId)
                    .Include(doc => doc.Documents)
                    .FirstOrDefaultAsync();

                var allDocuments = new List<Document>();
                allDocuments.AddRange(item.Documents);
                allDocuments.AddRange(itemTemplate.Documents);
                    
                if (allDocuments.Any()) return null;

                List<DocumentResponseDto> documentResponseList = new List<DocumentResponseDto>();

                foreach (var document in allDocuments)
                {
                    var fileBytes = await DownloadDocumentFromBlobStorage(document.BlobId);

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

                var fileBytes = await DownloadDocumentFromBlobStorage(document.BlobId);

                var documentResponse = new DocumentResponseDto
                {
                    Id = document.Id,
                    Name = document.DocumentType.Name,
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

        public async Task<string> UploadDocumentAsync(DocumentUploadDto document)
        {
            try
            {
                return "hello";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public Task AddDocumentToItemAsync(string documentId, string itemId)
        {
            throw new NotImplementedException();
        }

        public Task AddDocumentToItemTemplateAsync(string documentId, string itemTemplateId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveDocumentFromItemAsync(string documentId, string itemId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveDocumentFromItemTemplateAsync(string documentId, string ItemTemplateId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteDocumentAsync(string documentId)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteDocumentFromItemAsync(Document document, string itemId)
        {
            try
            {
                var item = await _context.Items.Where(item => item.Id == itemId)
                    .Include(item => item.Documents)
                    .FirstOrDefaultAsync();

                item.Documents.Remove(document);
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
            var blobContainerEndpoint = "https://storageaccountinventory.blob.core.windows.net/item-documentation";
            BlobContainerClient blobContainerClient =
                new BlobContainerClient(new Uri(blobContainerEndpoint), new DefaultAzureCredential());

            using MemoryStream documentStream = new();
            
            var blobClient = blobContainerClient.GetBlobClient(blobId);

            await blobClient.DownloadToAsync(documentStream);

            return documentStream.ToArray();
        }
    }
}

