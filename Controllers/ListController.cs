using Inventory.Models;
using Inventory.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Inventory.Services;
using Inventory.Validations.ListValidations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Inventory.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ListController(
    IListService listService,
    IUserService userService,
    IListCreateValidator createValidator,
    IListUpdateValidator updateValidator)
    : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "Get all lists", Description = "Retrieves a list of all lists.")]
    [SwaggerResponse(200, "Success", typeof(IEnumerable<List>))]
    [SwaggerResponse(400, "Invalid request")]
    public async Task<ActionResult<IEnumerable<List>>> GetAllLists()
    {
        try
        {
            return Ok(await listService.GetAllListsAsync());
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get list", Description = "Retrieves a list.")]
    [SwaggerResponse(200, "Success", typeof(List))]
    [SwaggerResponse(400, "Invalid request")]
    [SwaggerResponse(404, "List not found")]
    public async Task<ActionResult<List>> GetList(string id)
    {
        try
        {
            var list = await listService.GetListByIdAsync(id);
            if (list == null)
            {
                return NotFound("List not found");
            }

            return Ok(list);
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpGet("ByUserId/{id}")]
    [SwaggerOperation(Summary = "Get lists added by user", Description = "Retrieves lists added by the user.")]
    [SwaggerResponse(200, "Success", typeof(IEnumerable<List>))]
    [SwaggerResponse(400, "Invalid request")]
    public async Task<ActionResult<IEnumerable<List>>> GetListsByUserId(string id, int page)
    {
        try
        {
            return Ok(await listService.GetAllListsByUserIdAsync(id, page));
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpGet("BySearchString/{searchString}")]
    [SwaggerOperation(Summary = "Get lists containing search string", Description = "Retrieves lists containing search string in title, WpId, serial number or description.")]
    [SwaggerResponse(200, "Success", typeof(IEnumerable<List>))]
    [SwaggerResponse(400, "Invalid request")]
    [SwaggerResponse(404, "User not found")]
    public async Task<ActionResult<IEnumerable<List>>> GetListsBySearchString(string searchString, int page, string userId)
    {
        try
        {
            var user = await userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }
            return Ok(await listService.GetAllListsBySearchStringAsync(searchString, page, userId));
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create a new list", Description = "Creates a new list.")]
    [SwaggerResponse(201, "List created", typeof(List))]
    [SwaggerResponse(400, "Invalid request")]
    public async Task<ActionResult<List>> CreateList(ListCreateDto listCreate)
    {
        var validationResult = await createValidator.ValidateAsync(listCreate);
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
            var listId = await listService.CreateListAsync(listCreate);
            if (listId == null)
            {
                return StatusCode(500);
            }

            var list = await listService.GetListByIdAsync(listId);

            return CreatedAtAction(nameof(GetList), new { id = listId }, list);
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpPost("AddItems")]
    [SwaggerOperation(Summary = "Add items to a list", Description = "Adds items to a list.")]
    [SwaggerResponse(200, "Items added", typeof(List))]
    [SwaggerResponse(400, "Invalid request")]
    [SwaggerResponse(404, "List not found")]
    public async Task<ActionResult<List>> AddItemsToList(IEnumerable<string> itemIds, string listId, bool? addSubItems)
    {
        try
        {
            var list = await listService.GetListByIdAsync(listId);
            if (list == null)
            {
                return NotFound("List not found");
            }

            await listService.AddItemsToListAsync(itemIds, listId, addSubItems);

            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpPost("RemoveItems")]
    [SwaggerOperation(Summary = "Remove items from a list", Description = "Removes items to a list.")]
    [SwaggerResponse(201, "Items removed", typeof(List))]
    [SwaggerResponse(400, "Invalid request")]
    [SwaggerResponse(404, "List not found")]
    public async Task<ActionResult<List>> RemoveItemsFromList(IEnumerable<string> itemIds, string listId, bool? removeSubItems)
    {
        try
        {
            var list = await listService.GetListByIdAsync(listId);
            if (list == null)
            {
                return NotFound("List not found");
            }

            await listService.RemoveItemsFromListAsync(itemIds, listId, removeSubItems);

            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update list", Description = "Updates a list.")]
    [SwaggerResponse(200, "List updated")]
    [SwaggerResponse(400, "Invalid request")]
    [SwaggerResponse(404, "List not found")]
    public async Task<IActionResult> UpdateList(string id, List listUpdate)
    {
        var validationResult = await updateValidator.ValidateAsync(listUpdate);
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
            if (id != listUpdate.Id)
            {
                return BadRequest("Id does not match");
            }

            var list = await listService.GetListByIdAsync(id);
            if (list == null)
            {
                return NotFound("List not found");
            }

            await listService.UpdateListAsync(listUpdate);

            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete list", Description = "Deletes an list.")]
    [SwaggerResponse(200, "List deleted")]
    [SwaggerResponse(400, "Invalid request")]
    [SwaggerResponse(404, "List not found")]
    public async Task<IActionResult> DeleteList(string id)
    {
        try
        {
            var list = await listService.GetListByIdAsync(id);
            if (list == null)
            {
                return NotFound("List not found");
            }

            await listService.DeleteListAsync(id);

            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }
}