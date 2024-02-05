using Inventory.Models;
using Inventory.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Inventory.Services;
using Inventory.Validations.PreCheckValidations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Inventory.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PreCheckController(
    IPreCheckService preCheckService,
    IPreCheckCreateValidator createValidator,
    IPreCheckUpdateValidator updateValidator)
    : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "Get all pre checks", Description = "Retrieves a list of all pre checks.")]
    [SwaggerResponse(200, "Success", typeof(IEnumerable<PreCheck>))]
    [SwaggerResponse(400, "Invalid request")]
    public async Task<ActionResult<IEnumerable<PreCheck>>> GetAllCategories()
    {
        try
        {
            return Ok(await preCheckService.GetAllPreChecksAsync());
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get pre check", Description = "Retrieves a pre check.")]
    [SwaggerResponse(200, "Success", typeof(PreCheck))]
    [SwaggerResponse(400, "Invalid request")]
    [SwaggerResponse(404, "Pre check not found")]
    public async Task<ActionResult<PreCheck>> GetPreCheck(string id)
    {
        try
        {
            var preCheck = await preCheckService.GetPreCheckByIdAsync(id);
            if (preCheck == null)
            {
                return NotFound("Pre check not found");
            }

            return Ok(preCheck);
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create a new pre check", Description = "Creates a new pre check.")]
    [SwaggerResponse(201, "Pre check created", typeof(PreCheck))]
    [SwaggerResponse(400, "Invalid request")]
    public async Task<ActionResult<PreCheck>> CreatePreCheck(PreCheckCreateDto preCheckCreate)
    {
        var validationResult = await createValidator.ValidateAsync(preCheckCreate);
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
            var preCheckId = await preCheckService.CreatePreCheckAsync(preCheckCreate);
            if (preCheckId == null)
            {
                return BadRequest("PreCheck creation failed");
            }

            var preCheck = await preCheckService.GetPreCheckByIdAsync(preCheckId);

            return CreatedAtAction(nameof(GetPreCheck), new { id = preCheckId }, preCheck);

        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update pre check", Description = "Updates a pre check.")]
    [SwaggerResponse(200, "PreCheck updated")]
    [SwaggerResponse(400, "Invalid request")]
    [SwaggerResponse(404, "PreCheck not found")]
    public async Task<IActionResult> UpdatePreCheck(string id, PreCheck preCheckUpdate)
    {
        var validationResult = await updateValidator.ValidateAsync(preCheckUpdate);
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
            if (id != preCheckUpdate.Id)
            {
                return BadRequest("Id does not match");
            }

            var preCheck = await preCheckService.GetPreCheckByIdAsync(id);
            if (preCheck == null)
            {
                return NotFound("Pre check not found");
            }

            await preCheckService.UpdatePreCheckAsync(preCheckUpdate);

            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete pre check", Description = "Deletes a pre check.")]
    [SwaggerResponse(200, "PreCheck deleted")]
    [SwaggerResponse(400, "Invalid request")]
    [SwaggerResponse(404, "PreCheck not found")]
    public async Task<IActionResult> DeletePreCheck(string id)
    {
        try
        {
            var preCheck = await preCheckService.GetPreCheckByIdAsync(id);
            if (preCheck == null)
            {
                return NotFound("Pre check not found");
            }

            await preCheckService.DeletePreCheckAsync(id);

            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }
}