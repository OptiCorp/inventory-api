using Inventory.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Inventory.Services;
using Inventory.Validations.SizeValidations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SizeController : ControllerBase
    {
        private readonly ISizeService _sizeService;
        private readonly ISizeCreateValidator _createValidator;
        private readonly ISizeUpdateValidator _updateValidator;

        public SizeController(ISizeService sizeService, ISizeCreateValidator createValidator, ISizeUpdateValidator updateValidator)
        {
            _sizeService = sizeService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }
        
        [HttpGet]
        [SwaggerOperation(Summary = "Get all categories", Description = "Retrieves a list of all categories.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<Size>))]
        public async Task<ActionResult<IEnumerable<Size>>> GetAllCategories()
        {
            return Ok(await _sizeService.GetAllSizesAsync());
        }
        
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get size", Description = "Retrieves a size.")]
        [SwaggerResponse(200, "Success", typeof(Size))]
        [SwaggerResponse(404, "Size not found")]
        public async Task<ActionResult<Size>> GetSize(string id)
        {
            var size = await _sizeService.GetSizeByIdAsync(id);
            if (size == null)
            {
                return NotFound("Size not found");
            }

            return Ok(size);
        }
        
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new size", Description = "Creates a new size.")]
        [SwaggerResponse(201, "Size created", typeof(Size))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<ActionResult<Size>> CreateSize(Size sizeCreate)
        {
            var validationResult = await _createValidator.ValidateAsync(sizeCreate);
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
            
            var sizeId = await _sizeService.CreateSizeAsync(sizeCreate);
            if (sizeId == null)
            {
                return BadRequest("Size creation failed");
            }

            var size = await _sizeService.GetSizeByIdAsync(sizeId);

            return CreatedAtAction(nameof(GetSize), new { id = sizeId }, size);
        }
        
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update size", Description = "Updates a size.")]
        [SwaggerResponse(200, "Size updated")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Size not found")]
        
        
        public async Task<IActionResult> UpdateSize(string id, Size sizeUpdate)
        {
            var validationResult = await _updateValidator.ValidateAsync(sizeUpdate);
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
            
            if (id != sizeUpdate.Id)
            {
                return BadRequest("Id does not match");
            }

            var size = await _sizeService.GetSizeByIdAsync(id);
            if (size == null)
            {
                return NotFound("Size not found");
            }

            await _sizeService.UpdateSizeAsync(sizeUpdate);

            return NoContent();
        }
        
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete size", Description = "Deletes a size.")]
        [SwaggerResponse(200, "Size deleted")]
        [SwaggerResponse(404, "Size not found")]
        public async Task<IActionResult> DeleteSize(string id)
        {
            var size = await _sizeService.GetSizeByIdAsync(id);
            if (size == null)
            {
                return NotFound("Size not found");
            }

            await _sizeService.DeleteSizeAsync(id);

            return NoContent();
        }
    }
}
