using Inventory.Models;
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
            return Ok(await _documentService.GetDocumentationByItemId(id));
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Upload documentation for item", Description = "Uploads documents for item")]
        [SwaggerResponse(201, "Document uploaded", typeof(Document))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<ActionResult> UploadDocument([FromForm] UploadDocumentDto document)
        {
            var item = await _itemService.GetItemByIdAsync(document.ItemId);
            if (item == null)
            {
                return NotFound("Item not found");
            }

            string[] newDocumentationIds = await _documentService.UploadDocumentationAsync(document);
            var documentations = new List<Document>();
            foreach (var id in newDocumentationIds)
            {
                Document newDocumentation = await _documentService.GetDocumentationById(id); 
            }

            return CreatedAtAction(nameof(PostDocumentation), new { ids = newDocumentationIds }, documentations);
        }

        [HttpDelete("{documentId}")]
        [SwaggerOperation(Summary = "Remove a document from an item", Description = "Removes a document from an item")]
        [SwaggerResponse(200, "Document removed")]
        [SwaggerResponse(404, "User or document not found")]
        public async Task<IActionResult> DeleteDocumentFromItem(string documentId, string itemId)
        {
            var document = await _documentService.GetDocumentationById(documentId);
            if (document == null)
            {
                return NotFound("Document not found");
            }
            if (document.ItemId != itemId)
            {
                return NotFound("Document not in item");
            }

            await _documentService.DeleteDocumentFromItem(document);

            return Ok();
        }
    }
}
