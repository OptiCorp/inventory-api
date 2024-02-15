using System.ComponentModel.DataAnnotations;
using Inventory.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Inventory.Services;

namespace Inventory.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LogEntryController(ILogEntryService logEntryService, IItemService itemService, IItemTemplateService itemTemplateService) : ControllerBase
{
    [HttpGet("GetLogEntriesByItemId/{id}")]
    [SwaggerOperation(Summary = "Get log entries from item", Description = "Retrieves log entries from item.")]
    [SwaggerResponse(200, "Success", typeof(LogEntry))]
    [SwaggerResponse(400, "Invalid request")]
    [SwaggerResponse(404, "Item not found")]
    public async Task<IActionResult> GetLogEntriesByItemId(string id, [Required] int page, bool? includeTemplateEntries)
    {
        try
        {
            var item = await itemService.GetItemByIdAsync(id);
            if (item == null)
            {
                return NotFound("Item not found");
            }

            return Ok(await logEntryService.GetLogEntriesByItemIdAsync(id, page, includeTemplateEntries));
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpGet("GetLogEntriesByItemTemplateId/{templateId}")]
    [SwaggerOperation(Summary = "Get log entries from item and template by template id", Description = "Retrieves log entries from item and template by template id.")]
    [SwaggerResponse(200, "Success", typeof(LogEntry))]
    [SwaggerResponse(400, "Invalid request")]
    [SwaggerResponse(404, "Item template not found")]
    public async Task<IActionResult> GetLogEntriesByItemTemplateId(string id, [Required] int page)
    {
        try
        {
            var itemTemplate = await itemTemplateService.GetItemTemplateByIdAsync(id);
            if (itemTemplate == null)
            {
                return NotFound("Item template not found");
            }

            return Ok(await logEntryService.GetLogEntriesByItemTemplateIdAsync(id, page));
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }
}