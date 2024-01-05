using FluentValidation;
using Inventory.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Inventory.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly IListService _listService;

        public ItemController(IItemService itemService, IListService listService)
        {
            _itemService = itemService;
            _listService = listService;
        }
        
        [HttpGet]
        [SwaggerOperation(Summary = "Get all items", Description = "Retrieves a list of all items.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<Item>))]
        public async Task<ActionResult<IEnumerable<Item>>> GetAllItems()
        {
            return Ok(await _itemService.GetAllItemsAsync());
        }
        
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get item", Description = "Retrieves an item.")]
        [SwaggerResponse(200, "Success", typeof(Item))]
        [SwaggerResponse(404, "Item not found")]
        public async Task<ActionResult<Item>> GetItem(string id)
        {
            var item = await _itemService.GetItemByIdAsync(id);
            if (item == null)
            {
                return NotFound("Item not found");
            }

            return Ok(item);
        }
        
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new item", Description = "Creates a new item.")]
        [SwaggerResponse(201, "Item created", typeof(Item))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<ActionResult<Item>> CreateItem(List<Item> itemCreateList, [FromServices] IValidator<ItemCreateDto> validator)
        {
            foreach (var itemCreate in itemCreateList)
            {
                var validationResult = await validator.ValidateAsync(itemCreateDto);
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
            }
            
            var itemIds = await _itemService.CreateItemAsync(itemCreateList);
            if (itemIds == null)
            {
                return BadRequest("Item creation failed");
            }

            var items = new List<Item>();
            foreach (var itemId in itemIds)
            {
                var item = await _itemService.GetItemByIdAsync(itemId);
                items.Add(item);
            }
            
            return CreatedAtAction(nameof(GetItem), new { ids = itemIds }, items);
        }

        [HttpGet("BySearchString/{searchString}")]
        [SwaggerOperation(Summary = "Get items containing search string", Description = "Retrieves items containing search string in WpId, serial number or description.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<Item>))]
        public async Task<ActionResult<IEnumerable<Item>>> GetItemsBySearchString(string searchString, int page, string? type)
        {
            return Ok(await _itemService.GetAllItemsBySearchStringAsync(searchString, page, type));
        }
        
        [HttpGet("BySearchStringNotInList/{searchString}")]
        [SwaggerOperation(Summary = "Get items not in list containing search string", Description = "Retrieves items not in list containing search string in WpId, serial number or description.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<Item>))]
        [SwaggerResponse(404, "List not found")]
        public async Task<ActionResult<IEnumerable<Item>>> GetItemsNotInLIstBySearchString(string searchString, string listId, int page)
        {
            var list = await _listService.GetListByIdAsync(listId);
            if (list == null)
            {
                return NotFound("List not found");
            }
            return Ok(await _itemService.GetAllItemsNotInListBySearchStringAsync(searchString, listId, page));
        }
        
        [HttpGet("ByUserId/{id}")]
        [SwaggerOperation(Summary = "Get items added by user", Description = "Retrieves items added by the user.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<Item>))]
        public async Task<ActionResult<IEnumerable<Item>>> GetItemsByUserId(string id, int page)
        {
            return Ok(await _itemService.GetAllItemsByUserIdAsync(id, page));
        }
        
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update item", Description = "Updates an item.")]
        [SwaggerResponse(200, "Item updated")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Item not found")]
        public async Task<IActionResult> UpdateItem(string id, string updatedById, Item itemUpdate, [FromServices] IValidator<ItemUpdateDto> validator)
        {
            var validationResult = await validator.ValidateAsync(itemUpdate);
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
            
            if (id != itemUpdate.Id)
            {
                return BadRequest("Id does not match");
            }

            var item = await _itemService.GetItemByIdAsync(id);
            if (item == null)
            {
                return NotFound("Item not found");
            }

            await _itemService.UpdateItemAsync(updatedById, itemUpdate);

            return NoContent();
        }
        
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete item", Description = "Deletes an item.")]
        [SwaggerResponse(200, "Item deleted")]
        [SwaggerResponse(404, "Item not found")]
        public async Task<IActionResult> DeleteItem(string id)
        {
            var item = await _itemService.GetItemByIdAsync(id);
            if (item == null)
            {
                return NotFound("Item not found");
            }

            await _itemService.DeleteItemAsync(id);

            return NoContent();
        }
        
        [HttpGet("IsWpIdUnique/{wpId}")]
        [SwaggerOperation(Summary = "Unique WellPartner Id check", Description = "Checks if WellPartner Id is unique.")]
        [SwaggerResponse(200, "Success", typeof(bool))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<ActionResult<bool>> IsIdUnique(string wpId)
        {
            return await _itemService.IsWpIdUnique(wpId);
        }
    }
}
