using Azure.Identity;
using Azure.Storage.Blobs;
using Inventory.Models;
using Inventory.Utilities.DocumentUtilities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly InventoryDbContext _context;
        private readonly IDocumentUtilities _documentUtilities;

        public DocumentService(InventoryDbContext context, IDocumentUtilities documentUtilities)
        {
            _context = context;
            _documentUtilities = documentUtilities;
        }

        public async Task<IEnumerable<UploadDocumentDto>> GetDocumentsByItemId(string id)
        {
            var containerEndpoint = Environment.GetEnvironmentVariable("blobContainerEndpoint");
            BlobContainerClient containerClient =
                new BlobContainerClient(new Uri(containerEndpoint), new DefaultAzureCredential());
            var documentList = new List<Document>();
            
            var documents = _context.Documents.Where(d => d.ItemId == id);

            foreach (var document in documents)
            {
                var stream = new MemoryStream();

                var blobClient = containerClient.GetBlobClient(document.BlobRef);

                await blobClient.DownloadToAsync(stream);

                var documentResponse =
                    _documentUtilities.DocumentToResponseDto(document, stream.ToArray());
                
                documentList.Add(documentResponse);
            }

            return documentList;
        }

        public async Task<Document> GetDocumentById(string id)
        {
            return await _context.Documents.FirstOrDefaultAsync(document => document.Id == id);
        }

        public async Task<string[]> UploadDocumentAsync(DocumentCreateDto document)
        {
            var containerEndpoint = Environment.GetEnvironmentVariable("blobContainerEndpoint");

            BlobContainerClient containerClient =
                new BlobContainerClient(new Uri(containerEndpoint), new DefaultAzureCredential());

            var documentIds = new List<string>();
            
            foreach (var file in document.Files)
            {
                var newDocument = new Document
                {
                    ItemId = document.ItemId,
                    Name = file.FileName,
                    ContentType = file.ContentType,
                    BlobRef = Guid.NewGuid().ToString()
                };
                
                try
                {
                    await containerClient.CreateIfNotExistsAsync();

                    using (MemoryStream stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        stream.Position = 0;
                        await containerClient.UploadBlobAsync(newDocument.BlobRef, stream);
                    }
                }
                catch (Exception e)
                {
                    throw;
                }

                await _context.Documents.AddAsync(newDocument);
                await _context.SaveChangesAsync();
                
                documentIds.Add(newDocument.Id);
                
            }
            
            return documentIds.ToArray();
        }

        public async Task DeleteDocumentFromItem(Document document)
        {
            var containerEndpoint = Environment.GetEnvironmentVariable("blobContainerEndpoint");
            
            BlobContainerClient containerClient =
                new BlobContainerClient(new Uri(containerEndpoint), new DefaultAzureCredential());
            
            var blobClient = containerClient.GetBlobClient(document.BlobRef);
            
            await blobClient.DeleteAsync();
                
            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();
            
        }
    }
}

