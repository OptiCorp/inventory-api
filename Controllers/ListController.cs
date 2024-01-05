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
    public class ListController : ControllerBase
    {
        private readonly IListService _listService;
        private readonly IUserService _userService;

        public ListController(IListService listService, IUserService userService)
        {
            _listService = listService;
            _userService = userService;
        }
        
        [HttpGet]
        [SwaggerOperation(Summary = "Get all lists", Description = "Retrieves a list of all lists.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<List>))]
        public async Task<ActionResult<IEnumerable<List>>> GetAllLists()
        {
            return Ok(await _listService.GetAllListsAsync());
        }
        
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get list", Description = "Retrieves a list.")]
        [SwaggerResponse(200, "Success", typeof(List))]
        [SwaggerResponse(404, "List not found")]
        public async Task<ActionResult<List>> GetList(string id)
        {
            var list = await _listService.GetListByIdAsync(id);
            if (list == null)
            {
                return NotFound("List not found");
            }

            return Ok(list);
        }
        
        [HttpGet("ByUserId/{id}")]
        [SwaggerOperation(Summary = "Get lists added by user", Description = "Retrieves lists added by the user.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<List>))]
        public async Task<ActionResult<IEnumerable<List>>> GetListsByUserId(string id, int page)
        {
            return Ok(await _listService.GetAllListsByUserIdAsync(id, page));
        }
        
        [HttpGet("BySearchString/{searchString}")]
        [SwaggerOperation(Summary = "Get lists containing search string", Description = "Retrieves lists containing search string in title, WpId, serial number or description.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<List>))]
        [SwaggerResponse(404, "User not found")]
        public async Task<ActionResult<IEnumerable<List>>> GetListsBySearchString(string searchString, int page, string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }
            return Ok(await _listService.GetAllListsBySearchStringAsync(searchString, page, userId));
        }
        
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new list", Description = "Creates a new list.")]
        [SwaggerResponse(201, "List created", typeof(List))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<ActionResult<List>> CreateList(List listCreate, [FromServices] IValidator<ListCreateDto> validator)
        {
            var validationResult = await validator.ValidateAsync(listCreate);
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
            
            var listId = await _listService.CreateListAsync(listCreate);
            if (listId == null)
            {
                return StatusCode(500);
            }

            var list = await _listService.GetListByIdAsync(listId);

            return CreatedAtAction(nameof(GetList), new { id = listId }, list);
        }
        
        [HttpPost("AddItems")]
        [SwaggerOperation(Summary = "Add items to a list", Description = "Adds items to a list.")]
        [SwaggerResponse(200, "Items added", typeof(List))]
        [SwaggerResponse(404, "List not found")]
        public async Task<ActionResult<List>> AddItemsToList(IEnumerable<string> itemIds, string listId)
        {
            var list = await _listService.GetListByIdAsync(listId);
            if (list == null)
            {
                return NotFound("List not found");
            }

            await _listService.AddItemsToListAsync(itemIds, listId);

            return NoContent();
        }
        
        [HttpPost("RemoveItems")]
        [SwaggerOperation(Summary = "Remove items from a list", Description = "Removes items to a list.")]
        [SwaggerResponse(201, "Items removed", typeof(List))]
        [SwaggerResponse(404, "List not found")]
        public async Task<ActionResult<List>> RemoveItemsFromList(IEnumerable<string> itemIds, string listId)
        {
            var list = await _listService.GetListByIdAsync(listId);
            if (list == null)
            {
                return NotFound("List not found");
            }

            await _listService.RemoveItemsFromListAsync(itemIds, listId);

            return NoContent();
        }
        
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update list", Description = "Updates a list.")]
        [SwaggerResponse(200, "List updated")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "List not found")]
        public async Task<IActionResult> UpdateList(string id, List listUpdate, [FromServices] IValidator<ListUpdateDto> validator)
        {
            var validationResult = await validator.ValidateAsync(listUpdate);
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
            
            if (id != listUpdate.Id)
            {
                return BadRequest("Id does not match");
            }

            var list = await _listService.GetListByIdAsync(id);
            if (list == null)
            {
                return NotFound("List not found");
            }

            await _listService.UpdateListAsync(listUpdate);

            return NoContent();
        }
        
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete list", Description = "Deletes an list.")]
        [SwaggerResponse(200, "List deleted")]
        [SwaggerResponse(404, "List not found")]
        public async Task<IActionResult> DeleteList(string id)
        {
            var list = await _listService.GetListByIdAsync(id);
            if (list == null)
            {
                return NotFound("List not found");
            }

            await _listService.DeleteListAsync(id);

            return NoContent();
        }
    }
}
