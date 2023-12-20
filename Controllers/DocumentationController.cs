using Inventory.Models;
using Inventory.Models.DTOs.DocumentationDtos;
using Inventory.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentationController : ControllerBase
    {
        private readonly IDocumentationService _documentationService;
        private readonly IItemService _itemService;

        public DocumentationController(IDocumentationService documentationService, IItemService itemService)
        {
            _documentationService = documentationService;
            _itemService = itemService;
        }

        [HttpGet("id")]
        [SwaggerOperation(Summary = "Get documentation by item ID", Description = "Retrives all files related to an item")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<DocumentationResponseDto>))]
        public async Task<ActionResult<IEnumerable<DocumentationResponseDto>>> GetDocumentationByItemId(string id)
        {
            var item = await _itemService.GetItemByIdAsync(id);
            if (item == null)
            {
                return NotFound("Item not found");
            }
            return Ok(await _documentationService.GetDocumentationByItemId(id));
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Upload documentation for item", Description = "Uploads documents for item")]
        [SwaggerResponse(201, "Document uploaded", typeof(Documentation))]
        [SwaggerResponse(400, "Invalis request")]
        public async Task<ActionResult> PostDocumentation([FromForm] DocumentationCreateDto documentation)
        {
            var item = await _itemService.GetItemByIdAsync(documentation.ItemId);
            if (item == null)
            {
                return NotFound("Item not found");
            }

            string newDocumentationId = await _documentationService.UploadDocumentationAsync(documentation);
            Documentation newDocumentation = await _documentationService.GetDocumentationById(newDocumentationId);

            return CreatedAtAction(nameof(PostDocumentation), new { id = newDocumentationId }, newDocumentation);
            // return Ok();
        }
    }
}
