using Inventory.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Inventory.Services;
using Inventory.Validations.ItemTemplateValidations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemTemplateController : ControllerBase
    {
        private readonly IItemTemplateService _itemTemplateService;
        private readonly IItemTemplateCreateValidator _createValidator;
        private readonly IItemTemplateUpdateValidator _updateValidator;

        public ItemTemplateController(IItemTemplateService itemTemplateService, IItemTemplateCreateValidator createValidator, IItemTemplateUpdateValidator updateValidator)
        {
            _itemTemplateService = itemTemplateService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }
        
        [HttpGet]
        [SwaggerOperation(Summary = "Get all item templates", Description = "Retrieves a list of all item templates.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<ItemTemplate>))]
        public async Task<ActionResult<IEnumerable<ItemTemplate>>> GetAllItemTemplates()
        {
            return Ok(await _itemTemplateService.GetAllItemTemplatesAsync());
        }
        
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get item template", Description = "Retrieves an item template.")]
        [SwaggerResponse(200, "Success", typeof(ItemTemplate))]
        [SwaggerResponse(404, "Item template not found")]
        public async Task<ActionResult<ItemTemplate>> GetItemTemplate(string id)
        {
            var itemTemplate = await _itemTemplateService.GetItemTemplateByIdAsync(id);
            if (itemTemplate == null)
            {
                return NotFound("Item template not found");
            }

            return Ok(itemTemplate);
        }
        
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new item template", Description = "Creates a new item template.")]
        [SwaggerResponse(201, "Item template created", typeof(ItemTemplate))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<ActionResult<ItemTemplate>> CreateItemTemplate(ItemTemplate itemTemplateCreate)
        {
            var validationResult = await _createValidator.ValidateAsync(itemTemplateCreate);
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
            
            var itemTemplateId = await _itemTemplateService.CreateItemTemplateAsync(itemTemplateCreate);
            if (itemTemplateId == null)
            {
                return BadRequest("Item template creation failed");
            }

            var itemTemplate = await _itemTemplateService.GetItemTemplateByIdAsync(itemTemplateId);

            return CreatedAtAction(nameof(GetItemTemplate), new { id = itemTemplateId }, itemTemplate);
        }
        
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update item template", Description = "Updates an item template.")]
        [SwaggerResponse(200, "Item template updated")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Item template not found")]
        public async Task<IActionResult> UpdateItemTemplate(string id, ItemTemplate itemTemplateUpdate)
        {
            var validationResult = await _updateValidator.ValidateAsync(itemTemplateUpdate);
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
            
            if (id != itemTemplateUpdate.Id)
            {
                return BadRequest("Id does not match");
            }

            var itemTemplate = await _itemTemplateService.GetItemTemplateByIdAsync(id);
            if (itemTemplate == null)
            {
                return NotFound("Item template not found");
            }

            await _itemTemplateService.UpdateItemTemplateAsync(itemTemplateUpdate);

            return NoContent();
        }
        
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete item template", Description = "Deletes an item template.")]
        [SwaggerResponse(200, "Item template deleted")]
        [SwaggerResponse(404, "Item template not found")]
        public async Task<IActionResult> DeleteItemTemplate(string id)
        {
            var itemTemplate = await _itemTemplateService.GetItemTemplateByIdAsync(id);
            if (itemTemplate == null)
            {
                return NotFound("Item template not found");
            }

            await _itemTemplateService.DeleteItemTemplateAsync(id);

            return NoContent();
        }
    }
}
