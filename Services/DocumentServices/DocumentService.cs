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
                var documentList = new List<DocumentResponseDto>();
                var containerEndpoint = Environment.GetEnvironmentVariable("blobContainerEndpoint");
                if (containerEndpoint != null)
                {
                    BlobContainerClient containerClient =
                        new BlobContainerClient(new Uri(containerEndpoint), new DefaultAzureCredential());

                    var item = await _context.Items.Where(item => item.Id == id)
                        .Include(item => item.Documents)!.ThenInclude(doc => doc.DocumentType).FirstOrDefaultAsync();

                    var documents = item?.Documents;

                    if (documents != null)
                        foreach (var document in documents)
                        {
                            try
                            {
                                using (MemoryStream stream = new MemoryStream())
                                {
                                    var blobClient = containerClient.GetBlobClient(document.BlobId);

                                    await blobClient.DownloadToAsync(stream);

                                    var documentResponse = new DocumentResponseDto()
                                    {
                                        Id = document.Id,
                                        Name = document.DocumentType?.Name,
                                        BlobId = document.BlobId,
                                        ContentType = document.ContentType,
                                        Bytes = stream.ToArray()
                                    };

                                    documentList.Add(documentResponse);
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                                throw;
                            }
                        }

                }
                return documentList;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Document?> GetDocumentByIdAsync(string id)
        {
            try
            {
                return await _context.Documents.FirstOrDefaultAsync(document => document.Id == id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<string?> UploadDocumentAsync(DocumentUploadDto document)
        {
            try
            {
                var newDocument = new Document
                {
                    DocumentTypeId = document.DocumentTypeId,
                    BlobId = Guid.NewGuid().ToString(),
                    ContentType = document.File?.ContentType
                };

                var item = await _context.Items.Where(item => item.Id == document.ItemId)
                    .Include(item => item.Documents)
                    .FirstOrDefaultAsync();
                item?.Documents?.Add(newDocument);
                await _context.SaveChangesAsync();

                return newDocument.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task DeleteDocumentFromItemAsync(Document document, string itemId)
        {
            try
            {
                var item = await _context.Items.Where(item => item.Id == itemId)
                    .Include(item => item.Documents)
                    .FirstOrDefaultAsync();

                item?.Documents?.Remove(document);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}

