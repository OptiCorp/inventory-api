using Inventory.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Inventory.Services;
using Inventory.Validations.DocumentTypeValidations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentTypeController : ControllerBase
    {
        private readonly IDocumentTypeService _documentTypeService;
        private readonly IDocumentTypeCreateValidator _createValidator;
        private readonly IDocumentTypeUpdateValidator _updateValidator;

        public DocumentTypeController(IDocumentTypeService documentTypeService, IDocumentTypeCreateValidator createValidator, IDocumentTypeUpdateValidator updateValidator)
        {
            _documentTypeService = documentTypeService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }
        
        [HttpGet]
        [SwaggerOperation(Summary = "Get all document types", Description = "Retrieves a list of all document types.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<DocumentType>))]
        public async Task<ActionResult<IEnumerable<DocumentType>>> GetAllDocumentTypes()
        {
            return Ok(await _documentTypeService.GetAllDocumentTypesAsync());
        }
        
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get document type", Description = "Retrieves a document type.")]
        [SwaggerResponse(200, "Success", typeof(DocumentType))]
        [SwaggerResponse(404, "Document type not found")]
        public async Task<ActionResult<DocumentType>> GetDocumentType(string id)
        {
            var documentType = await _documentTypeService.GetDocumentTypeByIdAsync(id);
            if (documentType == null)
            {
                return NotFound("Document type not found");
            }

            return Ok(documentType);
        }
        
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new document type", Description = "Creates a new document type.")]
        [SwaggerResponse(201, "Document type created", typeof(DocumentType))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<ActionResult<DocumentType>> CreateDocumentType(DocumentType documentTypeCreate)
        {
            var validationResult = await _createValidator.ValidateAsync(documentTypeCreate);
            if (!validationResult.IsValid)
            {
                var modelStateDictionary = new ModelStateDictionary();
                foreach (var failure in validationResult.Errors)
                {
                    modelStateDictionary.AddModelError(
                        failure.PropertyName,
                        failure.ErrorMessage
                    );
                }
                return ValidationProblem(modelStateDictionary);
            }
            
            var documentTypeId = await _documentTypeService.CreateDocumentTypeAsync(documentTypeCreate);
            if (documentTypeId == null)
            {
                return BadRequest("Document type creation failed");
            }

            var documentType = await _documentTypeService.GetDocumentTypeByIdAsync(documentTypeId);

            return CreatedAtAction(nameof(GetDocumentType), new { id = documentTypeId }, documentType);
        }
        
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update document type", Description = "Updates a document type.")]
        [SwaggerResponse(200, "Document type updated")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Document type not found")]
        public async Task<IActionResult> UpdateDocumentType(string id, DocumentType documentTypeUpdate)
        {
            var validationResult = await _updateValidator.ValidateAsync(documentTypeUpdate);
            if (!validationResult.IsValid)
            {
                var modelStateDictionary = new ModelStateDictionary();
                foreach (var failure in validationResult.Errors)
                {
                    modelStateDictionary.AddModelError(
                        failure.PropertyName,
                        failure.ErrorMessage
                    );
                }
                return ValidationProblem(modelStateDictionary);
            }
            
            if (id != documentTypeUpdate.Id)
            {
                return BadRequest("Id does not match");
            }

            var documentType = await _documentTypeService.GetDocumentTypeByIdAsync(id);
            if (documentType == null)
            {
                return NotFound("Document type not found");
            }

            await _documentTypeService.UpdateDocumentTypeAsync(documentTypeUpdate);

            return NoContent();
        }
        
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete document type", Description = "Deletes a document type.")]
        [SwaggerResponse(200, "Document type deleted")]
        [SwaggerResponse(404, "Document type not found")]
        public async Task<IActionResult> DeleteDocumentType(string id)
        {
            var documentType = await _documentTypeService.GetDocumentTypeByIdAsync(id);
            if (documentType == null)
            {
                return NotFound("Document type not found");
            }

            await _documentTypeService.DeleteDocumentTypeAsync(id);

            return NoContent();
        }
    }
}