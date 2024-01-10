using Inventory.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Inventory.Services;
using Inventory.Validations.PreCheckValidations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreCheckController : ControllerBase
    {
        private readonly IPreCheckService _preCheckService;
        private readonly IPreCheckCreateValidator _createValidator;
        private readonly IPreCheckUpdateValidator _updateValidator;

        public PreCheckController(IPreCheckService preCheckService, IPreCheckCreateValidator createValidator, IPreCheckUpdateValidator updateValidator)
        {
            _preCheckService = preCheckService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }
        
        [HttpGet]
        [SwaggerOperation(Summary = "Get all pre checks", Description = "Retrieves a list of all pre checks.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<PreCheck>))]
        public async Task<ActionResult<IEnumerable<PreCheck>>> GetAllCategories()
        {
            return Ok(await _preCheckService.GetAllPreChecksAsync());
        }
        
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get pre check", Description = "Retrieves a pre check.")]
        [SwaggerResponse(200, "Success", typeof(PreCheck))]
        [SwaggerResponse(404, "Pre check not found")]
        public async Task<ActionResult<PreCheck>> GetPreCheck(string id)
        {
            var preCheck = await _preCheckService.GetPreCheckByIdAsync(id);
            if (preCheck == null)
            {
                return NotFound("Pre check not found");
            }

            return Ok(preCheck);
        }
        
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new pre check", Description = "Creates a new pre check.")]
        [SwaggerResponse(201, "Pre check created", typeof(PreCheck))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<ActionResult<PreCheck>> CreatePreCheck(PreCheck preCheckCreate)
        {
            var validationResult = await _createValidator.ValidateAsync(preCheckCreate);
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
            
            var preCheckId = await _preCheckService.CreatePreCheckAsync(preCheckCreate);
            if (preCheckId == null)
            {
                return BadRequest("PreCheck creation failed");
            }

            var preCheck = await _preCheckService.GetPreCheckByIdAsync(preCheckId);

            return CreatedAtAction(nameof(GetPreCheck), new { id = preCheckId }, preCheck);
        }
        
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update pre check", Description = "Updates a pre check.")]
        [SwaggerResponse(200, "PreCheck updated")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "PreCheck not found")]
        public async Task<IActionResult> UpdatePreCheck(string id, PreCheck preCheckUpdate)
        {
            var validationResult = await _updateValidator.ValidateAsync(preCheckUpdate);
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
            
            if (id != preCheckUpdate.Id)
            {
                return BadRequest("Id does not match");
            }

            var preCheck = await _preCheckService.GetPreCheckByIdAsync(id);
            if (preCheck == null)
            {
                return NotFound("Pre check not found");
            }

            await _preCheckService.UpdatePreCheckAsync(preCheckUpdate);

            return NoContent();
        }
        
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete pre check", Description = "Deletes a pre check.")]
        [SwaggerResponse(200, "PreCheck deleted")]
        [SwaggerResponse(404, "PreCheck not found")]
        public async Task<IActionResult> DeletePreCheck(string id)
        {
            var preCheck = await _preCheckService.GetPreCheckByIdAsync(id);
            if (preCheck == null)
            {
                return NotFound("Pre check not found");
            }

            await _preCheckService.DeletePreCheckAsync(id);

            return NoContent();
        }
    }
}
