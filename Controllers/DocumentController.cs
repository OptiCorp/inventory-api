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
        private readonly IItemTemplateService _itemTemplateService;

        public DocumentController(IDocumentService documentService, IItemService itemService, IItemTemplateService itemTemplateService)
        {
            _documentService = documentService;
            _itemService = itemService;
            _itemTemplateService = itemTemplateService;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get a document by its ID", Description = "Retrieves a document with the specified ID")]
        [SwaggerResponse(200, "Success", typeof(DocumentResponseDto))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<ActionResult<DocumentResponseDto>> GetDocumentById(string id)
        {
            try
            {
                return Ok(await _documentService.GetDocumentByIdAsync(id));
            }
            catch (Exception e)
            {
                return BadRequest($"Something went wrong: {e.Message}");
            }
        }


        [HttpGet("ByItemId/{id}")]
        [SwaggerOperation(Summary = "Get documents by item ID", Description = "Retrieves all files related to an item")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<DocumentResponseDto>))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<ActionResult<IEnumerable<DocumentResponseDto>>> GetDocumentsByItemId(string id)
        {
            try
            {
                var item = await _itemService.GetItemByIdAsync(id);
                if (item == null)
                {
                    return NotFound("Item not found");
                }

                var response = await _documentService.GetDocumentsByItemIdAsync(id);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest($"Something went wrong: {e.Message}");
            }
        }

        [HttpPost("AddDocToItem/{itemId}")]
        [SwaggerOperation(Summary = "Upload documents for an item", Description = "Uploads documents for item")]
        [SwaggerResponse(201, "Documents uploaded", typeof(Document))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<ActionResult> AddDocumentToItem([FromForm] DocumentUploadDto document, string itemId)
        {
            try
            {
                var item = await _itemService.GetItemByIdAsync(itemId);
                if (item == null)
                {
                    return NotFound("Item not found");
                }

                await _documentService.AddDocumentToItemAsync(document, itemId);

                return Ok("Document was added to item");
            }
            catch (Exception e)
            {
                return BadRequest($"Something went wrong: {e.Message}");
            }
        }

        [HttpPost("AddDocToItemTemplate/{itemTemplateId}")]
        [SwaggerOperation(Summary = "Upload documents for aan item template", Description = "Uploads documents for item template")]
        [SwaggerResponse(201, "Documents uploaded", typeof(Document))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<ActionResult> AddDocumentToItemTemplate([FromForm] DocumentUploadDto document, string itemTemplateId)
        {
            try
            {
                var itemTemplate = _itemTemplateService.GetItemTemplateByIdAsync(itemTemplateId);
                if (itemTemplate == null)
                {

                    return NotFound("Item template not found");

                }

                await _documentService.AddDocumentToItemTemplateAsync(document, itemTemplateId);

                return Ok("Document was added to item template");

            }
            catch (Exception e)
            {
                return BadRequest($"Something went wrong: {e.Message}");
            }
        }

        [HttpDelete("{documentId}")]
        [SwaggerOperation(Summary = "Delete a document", Description = "Delete a document")]
        [SwaggerResponse(200, "Document removed")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Item or document not found")]
        public async Task<IActionResult> DeleteDocument(string documentId)
        {
            try
            {
                var document = await _documentService.GetDocumentByIdAsync(documentId);
                if (document == null)
                {
                    return NotFound("Document not found");
                }

                await _documentService.DeleteDocumentAsync(documentId);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest($"Something went wrong: {e.Message}");
            }
        }
    }
}