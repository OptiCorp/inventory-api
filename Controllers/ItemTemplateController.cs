using System.ComponentModel.DataAnnotations;
using Inventory.Models;
using Inventory.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Inventory.Services;
using Inventory.Validations.ItemTemplateValidations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Inventory.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ItemTemplateController(
    IItemTemplateService itemTemplateService,
    IItemTemplateCreateValidator createValidator,
    IItemTemplateUpdateValidator updateValidator)
    : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "Get all item templates", Description = "Retrieves a list of all item templates.")]
    [SwaggerResponse(200, "Success", typeof(IEnumerable<ItemTemplate>))]
    [SwaggerResponse(400, "Invalid request")]
    public async Task<ActionResult<IEnumerable<ItemTemplate>>> GetAllItemTemplates()
    {
        try
        {
            return Ok(await itemTemplateService.GetAllItemTemplatesAsync());
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get item template", Description = "Retrieves an item template.")]
    [SwaggerResponse(200, "Success", typeof(ItemTemplate))]
    [SwaggerResponse(400, "Invalid request")]
    [SwaggerResponse(404, "Item template not found")]
    public async Task<ActionResult<ItemTemplate>> GetItemTemplate(string id)
    {
        try
        {
            var itemTemplate = await itemTemplateService.GetItemTemplateByIdAsync(id);
            if (itemTemplate == null)
            {
                return NotFound("Item template not found");
            }

            return Ok(itemTemplate);
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpGet("BySearchString/{searchString}")]
    [SwaggerOperation(Summary = "Get item templates containing search string",
        Description = "Retrieves item templates containing search string in type or description.")]
    [SwaggerResponse(200, "Success", typeof(IEnumerable<ItemTemplate>))]
    [SwaggerResponse(400, "Invalid request")]
    public async Task<ActionResult<IEnumerable<ItemTemplate>>> GetItemTemplateBySearchString(string searchString,
        [Required] int page)
    {
        try
        {
            return Ok(await itemTemplateService.GetItemTemplateBySearchStringAsync(searchString, page));
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create a new item template", Description = "Creates a new item template.")]
    [SwaggerResponse(201, "Item template created", typeof(ItemTemplate))]
    [SwaggerResponse(400, "Invalid request")]
    public async Task<ActionResult<ItemTemplate>> CreateItemTemplate(ItemTemplateCreateDto itemTemplateCreate)
    {
        var validationResult = await createValidator.ValidateAsync(itemTemplateCreate);
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
            var itemTemplateId = await itemTemplateService.CreateItemTemplateAsync(itemTemplateCreate);
            if (itemTemplateId == null)
            {
                return BadRequest("Item template creation failed");
            }

            var itemTemplate = await itemTemplateService.GetItemTemplateByIdAsync(itemTemplateId);

            return CreatedAtAction(nameof(GetItemTemplate), new { id = itemTemplateId }, itemTemplate);
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update item template", Description = "Updates an item template.")]
    [SwaggerResponse(200, "Item template updated")]
    [SwaggerResponse(400, "Invalid request")]
    [SwaggerResponse(404, "Item template not found")]
    public async Task<IActionResult> UpdateItemTemplate(string id, string updatedById, ItemTemplate itemTemplateUpdate)
    {
        var validationResult = await updateValidator.ValidateAsync(itemTemplateUpdate);
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
            if (id != itemTemplateUpdate.Id)
            {
                return BadRequest("Id does not match");
            }

            var itemTemplate = await itemTemplateService.GetItemTemplateByIdAsync(id);
            if (itemTemplate == null)
            {
                return NotFound("Item template not found");
            }

            await itemTemplateService.UpdateItemTemplateAsync(itemTemplateUpdate, updatedById);

            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete item template", Description = "Deletes an item template.")]
    [SwaggerResponse(200, "Item template deleted")]
    [SwaggerResponse(400, "Invalid request")]
    [SwaggerResponse(404, "Item template not found")]
    public async Task<IActionResult> DeleteItemTemplate(string id)
    {
        try
        {
            var itemTemplate = await itemTemplateService.GetItemTemplateByIdAsync(id);
            if (itemTemplate == null)
            {
                return NotFound("Item template not found");
            }

            await itemTemplateService.DeleteItemTemplateAsync(id);

            return NoContent();

        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }
}