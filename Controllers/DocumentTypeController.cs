using Inventory.Models;
using Inventory.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Inventory.Services;
using Inventory.Validations.DocumentTypeValidations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Inventory.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DocumentTypeController(
    IDocumentTypeService documentTypeService,
    IDocumentTypeCreateValidator createValidator,
    IDocumentTypeUpdateValidator updateValidator)
    : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "Get all document types", Description = "Retrieves a list of all document types.")]
    [SwaggerResponse(200, "Success", typeof(IEnumerable<DocumentType>))]
    [SwaggerResponse(400, "Invalid request")]
    public async Task<ActionResult<IEnumerable<DocumentType>>> GetAllDocumentTypes()
    {
        try
        {
            return Ok(await documentTypeService.GetAllDocumentTypesAsync());
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get document type", Description = "Retrieves a document type.")]
    [SwaggerResponse(200, "Success", typeof(DocumentType))]
    [SwaggerResponse(400, "Invalid request")]
    [SwaggerResponse(404, "Document type not found")]
    public async Task<ActionResult<DocumentType>> GetDocumentType(string id)
    {
        try
        {
            var documentType = await documentTypeService.GetDocumentTypeByIdAsync(id);
            if (documentType == null)
            {
                return NotFound("Document type not found");
            }

            return Ok(documentType);
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create a new document type", Description = "Creates a new document type.")]
    [SwaggerResponse(201, "Document type created", typeof(DocumentType))]
    [SwaggerResponse(400, "Invalid request")]
    public async Task<ActionResult<DocumentType>> CreateDocumentType(DocumentTypeCreateDto documentTypeCreate)
    {
        var validationResult = await createValidator.ValidateAsync(documentTypeCreate);
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

        try
        {
            var documentTypeId = await documentTypeService.CreateDocumentTypeAsync(documentTypeCreate);
            if (documentTypeId == null)
            {
                return BadRequest("Document type creation failed");
            }

            var documentType = await documentTypeService.GetDocumentTypeByIdAsync(documentTypeId);

            return CreatedAtAction(nameof(GetDocumentType), new { id = documentTypeId }, documentType);
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update document type", Description = "Updates a document type.")]
    [SwaggerResponse(200, "Document type updated")]
    [SwaggerResponse(400, "Invalid request")]
    [SwaggerResponse(404, "Document type not found")]
    public async Task<IActionResult> UpdateDocumentType(string id, DocumentType documentTypeUpdate)
    {
        var validationResult = await updateValidator.ValidateAsync(documentTypeUpdate);
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

        try
        {
            if (id != documentTypeUpdate.Id)
            {
                return BadRequest("Id does not match");
            }

            var documentType = await documentTypeService.GetDocumentTypeByIdAsync(id);
            if (documentType == null)
            {
                return NotFound("Document type not found");
            }

            await documentTypeService.UpdateDocumentTypeAsync(documentTypeUpdate);

            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete document type", Description = "Deletes a document type.")]
    [SwaggerResponse(200, "Document type deleted")]
    [SwaggerResponse(400, "Invalid request")]
    [SwaggerResponse(404, "Document type not found")]
    public async Task<IActionResult> DeleteDocumentType(string id)
    {
        try
        {
            var documentType = await documentTypeService.GetDocumentTypeByIdAsync(id);
            if (documentType == null)
            {
                return NotFound("Document type not found");
            }

            await documentTypeService.DeleteDocumentTypeAsync(id);

            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }
}