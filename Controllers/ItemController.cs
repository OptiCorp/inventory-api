using System.ComponentModel.DataAnnotations;
using Inventory.Models;
using Inventory.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Inventory.Services;
using Inventory.Validations.ItemValidations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Inventory.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ItemController(
    IItemService itemService,
    IItemCreateValidator createValidator,
    IItemUpdateValidator updateValidator)
    : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "Get all items", Description = "Retrieves a list of all items.")]
    [SwaggerResponse(200, "Success", typeof(IEnumerable<Item>))]
    [SwaggerResponse(400, "Invalid request")]
    public async Task<ActionResult<IEnumerable<Item>>> GetAllItems()
    {
        try
        {
            return Ok(await itemService.GetAllItemsAsync());
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get item", Description = "Retrieves an item.")]
    [SwaggerResponse(200, "Success", typeof(Item))]
    [SwaggerResponse(404, "Item not found")]
    public async Task<ActionResult<Item>> GetItem(string id)
    {
        try
        {
            var item = await itemService.GetItemByIdAsync(id);
            if (item == null)
            {
                return NotFound("Item not found");
            }

            return Ok(item);
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpGet("GetItemsChecklist")]
    [SwaggerOperation(Summary = "Get items for the Checklist app", Description = "Retrieves items for the Checklist app.")]
    [SwaggerResponse(200, "Success", typeof(Item))]
    [SwaggerResponse(404, "Item not found")]
    public async Task<ActionResult<Item>> GetItemsChecklist([Required] List<string> ids)
    {
        try
        {
            var items = await itemService.GetItemsByIdChecklistAsync(ids);
            if (items == null || items.Count == 0)
            {
                return NotFound("Item not found");
            }

            return Ok(items);
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create a new item", Description = "Creates a new item.")]
    [SwaggerResponse(201, "Item created", typeof(Item))]
    [SwaggerResponse(400, "Invalid request")]
    public async Task<ActionResult<Item>> CreateItem(List<ItemCreateDto> itemCreateList)
    {
        foreach (var itemCreate in itemCreateList)
        {
            var validationResult = await createValidator.ValidateAsync(itemCreate);
            if (validationResult.IsValid) continue;

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
            var itemIds = await itemService.CreateItemAsync(itemCreateList);
            if (itemIds is { Count: 0 } or null)
            {
                return BadRequest("Item creation failed");
            }

            var items = new List<Item>();
            foreach (var itemId in itemIds)
            {
                {
                    var item = await itemService.GetItemByIdAsync(itemId);
                    if (item != null) items.Add(item);
                }
            }
            await itemService.ItemsCreated(items);
            return CreatedAtAction(nameof(GetItem), new { id = itemIds }, items);
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpPost("AddChildItemToParent")]
    [SwaggerOperation(Summary = "Add child item to given item.", Description = "Add child item to given item.")]
    [SwaggerResponse(200, "Item updated")]
    [SwaggerResponse(400, "Invalid request")]
    [SwaggerResponse(404, "Item not found")]
    public async Task<IActionResult> AddChildItemToParent(string itemId, string childItemId)
    {
        try
        {
            var parentItem = await itemService.GetItemByIdAsync(itemId);
            var childItem = await itemService.GetItemByIdAsync(childItemId);
            if (parentItem == null)
            {
                return NotFound("Item not found");
            }

            if (childItem == null)
            {
                return NotFound("Child item not found");
            }

            if (parentItem.Id == childItem.Id)
            {
                return BadRequest("You cannot add an item as a child of itself. Please select a different item to be linked.");
            }

            await itemService.AddChildItemToParentAsync(itemId, childItemId);

            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpGet("BySearchString")]
    [SwaggerOperation(Summary = "Get items containing search string", Description = "Retrieves items containing search string in WpId, serial number or description.")]
    [SwaggerResponse(200, "Success", typeof(IEnumerable<Item>))]
    [SwaggerResponse(400, "Invalid request")]
    public async Task<ActionResult<IEnumerable<Item>>> GetItemsBySearchString(string? searchString, [Required] int page)
    {
        try
        {
            return Ok(await itemService.GetAllItemsBySearchStringAsync(searchString, page));
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpGet("ByUserId/{id}")]
    [SwaggerOperation(Summary = "Get items added by user", Description = "Retrieves items added by the user.")]
    [SwaggerResponse(200, "Success", typeof(IEnumerable<Item>))]
    [SwaggerResponse(400, "Invalid request")]
    public async Task<ActionResult<IEnumerable<Item>>> GetItemsByUserId(string id, int page)
    {
        try
        {
            return Ok(await itemService.GetAllItemsByUserIdAsync(id, page));
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update item", Description = "Updates an item.")]
    [SwaggerResponse(200, "Item updated")]
    [SwaggerResponse(400, "Invalid request")]
    [SwaggerResponse(404, "Item not found")]
    public async Task<IActionResult> UpdateItem(string id, string updatedById, Item itemUpdate)
    {
        var validationResult = await updateValidator.ValidateAsync(itemUpdate);
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
            if (id != itemUpdate.Id)
            {
                return BadRequest("Id does not match");
            }

            var item = await itemService.GetItemByIdAsync(id);
            if (item == null)
            {
                return NotFound("Item not found");
            }

            await itemService.UpdateItemAsync(updatedById, itemUpdate);

            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }
    [HttpPost("RemoveParentId")]
    [SwaggerOperation(Summary = "Removes parent id from item.", Description = "Removes parent id from item.")]
    [SwaggerResponse(200, "Item updated")]
    [SwaggerResponse(400, "Invalid request")]
    [SwaggerResponse(404, "Item not found")]
    public async Task<IActionResult> RemoveParentId(string itemId)
    {
        try
        {
            var item = await itemService.GetItemByIdAsync(itemId);
            if (item == null)
            {
                return NotFound("Item not found");
            }

            await itemService.RemoveParentIdAsync(itemId);

            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete item", Description = "Deletes an item.")]
    [SwaggerResponse(200, "Item deleted")]
    [SwaggerResponse(400, "Invalid request")]
    [SwaggerResponse(404, "Item not found")]
    public async Task<IActionResult> DeleteItem(string id, bool? deleteSubItems)
    {
        try
        {
            var item = await itemService.GetItemByIdAsync(id);
            if (item == null)
            {
                return NotFound("Item not found");
            }

            var deletedItems = await itemService.DeleteItemAsync(id, deleteSubItems);
            await itemService.ItemsDeleted(deletedItems);

            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpGet("IsWpIdUnique/{wpId}")]
    [SwaggerOperation(Summary = "Unique WellPartner Id check", Description = "Checks if WellPartner Id is unique.")]
    [SwaggerResponse(200, "Success", typeof(bool))]
    [SwaggerResponse(400, "Invalid request")]
    public async Task<ActionResult<bool>> IsWpIdUnique(string wpId)
    {
        try
        {
            return await itemService.IsWpIdUnique(wpId);
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpGet("IsSerialNumberUnique/{serialNumber}")]
    [SwaggerOperation(Summary = "Unique serial number check", Description = "Checks if serial number is unique.")]
    [SwaggerResponse(200, "Success", typeof(bool))]
    [SwaggerResponse(400, "Invalid request")]
    public async Task<ActionResult<bool>> IsSerialNumberUnique(string serialNumber)
    {
        try
        {
            return await itemService.IsSerialNumberUnique(serialNumber);
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }
}