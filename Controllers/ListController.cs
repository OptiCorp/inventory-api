using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Inventory.Models.DTOs.ListDTOs;
using Inventory.Services;

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
        [SwaggerResponse(200, "Success", typeof(IEnumerable<ListResponseDto>))]
        public async Task<ActionResult<IEnumerable<ListResponseDto>>> GetList()
        {
            return Ok(await _listService.GetAllListsAsync());
        }
        
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get list", Description = "Retrieves a list.")]
        [SwaggerResponse(200, "Success", typeof(ListResponseDto))]
        [SwaggerResponse(404, "List not found")]
        public async Task<ActionResult<ListResponseDto>> GetList(string id)
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
        [SwaggerResponse(200, "Success", typeof(IEnumerable<ListResponseDto>))]
        public async Task<ActionResult<IEnumerable<ListResponseDto>>> GetListByUserId(string id, int page)
        {
            return Ok(await _listService.GetAllListsByUserIdAsync(id, page));
        }
        
        [HttpGet("BySearchString/{searchString}")]
        [SwaggerOperation(Summary = "Get lists containing search string", Description = "Retrieves lists containing search string in title, WpId, serial number or description.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<ListResponseDto>))]
        [SwaggerResponse(404, "User not found")]
        public async Task<ActionResult<IEnumerable<ListResponseDto>>> GetListBySearchString(string searchString, int page, string userId)
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
        [SwaggerResponse(201, "List created", typeof(ListResponseDto))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<ActionResult<ListResponseDto>> PostList(ListCreateDto listCreateDto)
        {
            var listId = await _listService.CreateListAsync(listCreateDto);
            if (listId == null)
            {
                return StatusCode(500);
            }

            var list = await _listService.GetListByIdAsync(listId);

            return CreatedAtAction(nameof(GetList), new { id = listId }, list);
        }
        
        [HttpPost("AddItems")]
        [SwaggerOperation(Summary = "Add items to a list", Description = "Adds items to a list.")]
        [SwaggerResponse(200, "Items added", typeof(ListResponseDto))]
        [SwaggerResponse(404, "List not found")]
        public async Task<ActionResult<ListResponseDto>> AddItemsToList(IEnumerable<string> itemIds, string listId)
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
        [SwaggerResponse(201, "Items removed", typeof(ListResponseDto))]
        [SwaggerResponse(404, "List not found")]
        public async Task<ActionResult<ListResponseDto>> RemoveItemsFromList(IEnumerable<string> itemIds, string listId)
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
        public async Task<IActionResult> PutList(string id, ListUpdateDto listUpdateDto)
        {
            if (id != listUpdateDto.Id)
            {
                return BadRequest("Id does not match");
            }

            var list = await _listService.GetListByIdAsync(id);
            if (list == null)
            {
                return NotFound("List not found");
            }

            await _listService.UpdateListAsync(listUpdateDto);

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
