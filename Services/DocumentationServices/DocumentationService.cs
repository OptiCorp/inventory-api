using Azure.Identity;
using Azure.Storage.Blobs;
using Inventory.Models;
using Inventory.Models.DTOs.DocumentationDtos;
using Inventory.Utilities.DocumentationUtilities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Services
{
    public class DocumentationService : IDocumentationService
    {
        private readonly InventoryDbContext _context;
        private readonly IDocumentationUtilities _documentationUtilities;

        public DocumentationService(InventoryDbContext context, IDocumentationUtilities documentationUtilities)
        {
            _context = context;
            _documentationUtilities = documentationUtilities;
        }

        public async Task<IEnumerable<DocumentationResponseDto>> GetDocumentationByItemId(string id)
        {
            var containerEndpoint = Environment.GetEnvironmentVariable("blobContainerEndpoint");
            BlobContainerClient containerClient =
                new BlobContainerClient(new Uri(containerEndpoint), new DefaultAzureCredential());
            var documentationList = new List<DocumentationResponseDto>();

            var documents = _context.Documentations.Where(d => d.ItemId == id);

            foreach (var document in documents)
            {
                var stream = new MemoryStream();

                var blobClient = containerClient.GetBlobClient(document.BlobRef);

                await blobClient.DownloadToAsync(stream);

                var documentationResponse =
                    _documentationUtilities.DocumentationToResponseDto(document, stream.ToArray());
                
                documentationList.Add(documentationResponse);
            }

            return documentationList;
        }

        public async Task<Documentation> GetDocumentationById(string id)
        {
            return await _context.Documentations.FirstOrDefaultAsync(documentation => documentation.Id == id);
        }

        public async Task<string[]> UploadDocumentationAsync(DocumentationCreateDto documentation)
        {
            var containerEndpoint = Environment.GetEnvironmentVariable("blobContainerEndpoint");

            BlobContainerClient containerClient =
                new BlobContainerClient(new Uri(containerEndpoint), new DefaultAzureCredential());

            var documentationIds = new List<string>();
            
            foreach (var file in documentation.Files)
            {
                var newDocumentation = new Documentation
                {
                    ItemId = documentation.ItemId,
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
                        await containerClient.UploadBlobAsync(newDocumentation.BlobRef, stream);
                    }
                }
                catch (Exception e)
                {
                    throw;
                }

                await _context.Documentations.AddAsync(newDocumentation);
                await _context.SaveChangesAsync();
                
                documentationIds.Add(newDocumentation.Id);
                
            }
            
            return documentationIds.ToArray();
        }

        public async Task DeleteDocumentFromItem(Documentation document)
        {
            var containerEndpoint = Environment.GetEnvironmentVariable("blobContainerEndpoint");
            
            BlobContainerClient containerClient =
                new BlobContainerClient(new Uri(containerEndpoint), new DefaultAzureCredential());
            
            var blobClient = containerClient.GetBlobClient(document.BlobRef);
            
            await blobClient.DeleteAsync();
                
            _context.Documentations.Remove(document);
            await _context.SaveChangesAsync();
            
        }
    }
}

