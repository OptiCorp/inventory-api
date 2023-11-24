using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventory.Models;
using Swashbuckle.AspNetCore.Annotations;
using Inventory.Models.DTO;
using Inventory.Services;

namespace Inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly InventoryDbContext _context;
        private readonly IItemService _itemService;

        public ItemController(InventoryDbContext context, IItemService itemService)
        {
            _context = context;
            _itemService = itemService;
        }
        
        [HttpGet]
        [SwaggerOperation(Summary = "Get all items", Description = "Retrieves a list of all items.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<ItemResponseDto>))]
        public async Task<ActionResult<IEnumerable<ItemResponseDto>>> GetItem()
        {
            return Ok(await _itemService.GetAllItemsAsync());
        }
        
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get item", Description = "Retrieves an item.")]
        [SwaggerResponse(200, "Success", typeof(ItemResponseDto))]
        [SwaggerResponse(404, "Item not found")]
        public async Task<ActionResult<ItemResponseDto>> GetItem(string id)
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
        [SwaggerResponse(201, "Item created", typeof(ItemResponseDto))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<ActionResult<ItemResponseDto>> PostItem(ItemCreateDto itemCreateDto)
        {
            var itemId = await _itemService.CreateItemAsync(itemCreateDto);

            var item = await _itemService.GetItemByIdAsync(itemId);

            return CreatedAtAction(nameof(GetItem), new { id = itemId }, item);
        }
        
        [HttpGet("Children/{id}")]
        [SwaggerOperation(Summary = "Get an item's children", Description = "Retrieves a list of an item's children.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<ItemResponseDto>))]
        public async Task<ActionResult<IEnumerable<ItemResponseDto>>> GetChildren(string id)
        {
            return Ok(await _itemService.GetChildrenAsync(id));
        }

        [HttpGet("BySearchString/{searchString}")]
        [SwaggerOperation(Summary = "Get items containing search string", Description = "Retrieves items containing search string in WPId, serial number or description.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<ItemResponseDto>))]
        public async Task<ActionResult<IEnumerable<ItemResponseDto>>> GetItemBySearchString(string searchString)
        {
            return Ok(await _itemService.GetAllItemsBySearchStringAsync(searchString));
        }
        
        [HttpGet("ByUserId/{id}")]
        [SwaggerOperation(Summary = "Get items added by user", Description = "Retrieves items added by the user.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<ItemResponseDto>))]
        public async Task<ActionResult<IEnumerable<ItemResponseDto>>> GetItemByUserId(string id)
        {
            return Ok(await _itemService.GetAllItemsByUserIdAsync(id));
        }
        
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update item", Description = "Updates an item.")]
        [SwaggerResponse(200, "Item updated")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Item not found")]
        public async Task<IActionResult> PutItem(string id, ItemUpdateDto itemUpdateDto)
        {
            if (id != itemUpdateDto.Id)
            {
                return BadRequest("Id does not match");
            }

            var item = await _itemService.GetItemByIdAsync(id);
            if (item == null)
            {
                return NotFound("Item not found");
            }

            await _itemService.UpdateItemAsync(itemUpdateDto);

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
    }
}
