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
        [SwaggerOperation(Summary = "Get documentation by item ID", Description = "Retrieves all files related to an item")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<Document>))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<ActionResult<IEnumerable<Document>>> GetDocumentsByItemId(string id)
        {
            try
            {
                var item = await _itemService.GetItemByIdAsync(id);
                if (item == null)
                {
                    return NotFound("Item not found");
                }
                return Ok(item.Documents);
            }
            catch (Exception e)
            {
                return BadRequest($"Something went wrong: {e.Message}");
            }
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Upload documentation for item", Description = "Uploads documents for item")]
        [SwaggerResponse(201, "Document uploaded", typeof(Document))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<ActionResult> UploadDocument([FromForm] DocumentUploadDto document)
        {
            try
            {
                if (document.ItemId != null)
                {
                    var item = await _itemService.GetItemByIdAsync(document.ItemId);
                    if (item == null)
                    {
                        return NotFound("Item not found");
                    }
                }

                string? newDocumentId = await _documentService.UploadDocumentAsync(document);

                Document? newDocument = null;
                if (newDocumentId != null)
                {
                    newDocument = await _documentService.GetDocumentByIdAsync(newDocumentId); 
                
                }
                return CreatedAtAction(nameof(UploadDocument), new { id = newDocumentId }, newDocument);
            }
            catch (Exception e)
            {
                return BadRequest($"Something went wrong: {e.Message}");
            }
        }

        [HttpDelete("{documentId}")]
        [SwaggerOperation(Summary = "Remove a document from an item", Description = "Removes a document from an item")]
        [SwaggerResponse(200, "Document removed")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "User or document not found")]
        public async Task<IActionResult> DeleteDocumentFromItem(string documentId, string itemId)
        {
            try
            {
                var document = await _documentService.GetDocumentByIdAsync(documentId);
                if (document == null)
                {
                    return NotFound("Document not found");
                }

                await _documentService.DeleteDocumentFromItemAsync(document, itemId);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest($"Something went wrong: {e.Message}");
            }
        }
    }
}
