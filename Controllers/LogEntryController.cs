using System.ComponentModel.DataAnnotations;
using Inventory.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Inventory.Services;

namespace Inventory.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LogEntryController(ILogEntryService logEntryService) : ControllerBase
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
    public async Task<IActionResult> GetLogEntriesByItemTemplateId(string templateId, [Required] int page)
    {
        try
        {
            var logEntries = await logEntryService.GetLogEntriesByItemTemplateIdAsync(templateId, page);
            if (!logEntries.Any())
            {
                return NotFound("Item template not found");
            }

            return Ok(logEntries);
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }
}