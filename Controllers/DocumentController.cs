using Inventory.Models;
using Inventory.Models.DocumentDtos;
using Inventory.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IItemService _itemService;

        public DocumentController(IDocumentService documentService, IItemService itemService)
        {
            _documentService = documentService;
            _itemService = itemService;
        }

        [HttpGet("ByItemId/{id}")]
        [SwaggerOperation(Summary = "Get documentation by item ID", Description = "Retrives all files related to an item")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<Document>))]
        public async Task<ActionResult<IEnumerable<Document>>> GetDocumentsByItemId(string id)
        {
            var item = await _itemService.GetItemByIdAsync(id);
            if (item == null)
            {
                return NotFound("Item not found");
            }

            return Ok(item.Documents);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Upload documentation for item", Description = "Uploads documents for item")]
        [SwaggerResponse(201, "Document uploaded", typeof(Document))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<ActionResult> UploadDocument([FromForm] DocumentUploadDto document)
        {
            var item = await _itemService.GetItemByIdAsync(document.ItemId);
            if (item == null)
            {
                return NotFound("Item not found");
            }

            string newDocumentId = await _documentService.UploadDocumentAsync(document);
            
            Document newDocument = await _documentService.GetDocumentByIdAsync(newDocumentId); 
            

            return CreatedAtAction(nameof(UploadDocument), new { id = newDocumentId }, newDocument);
        }

        [HttpDelete("{documentId}")]
        [SwaggerOperation(Summary = "Remove a document from an item", Description = "Removes a document from an item")]
        [SwaggerResponse(200, "Document removed")]
        [SwaggerResponse(404, "User or document not found")]
        public async Task<IActionResult> DeleteDocumentFromItem(string documentId, string itemId)
        {
            var document = await _documentService.GetDocumentByIdAsync(documentId);
            if (document == null)
            {
                return NotFound("Document not found");
            }

            await _documentService.DeleteDocumentFromItemAsync(document, itemId);

            return Ok();
        }
    }
}
