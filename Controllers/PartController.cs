using System;
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
    public class PartController : ControllerBase
    {
        private readonly InventoryDbContext _context;

        private readonly IPartService _partService;

        private readonly ISubassemblyService _subassemblyService;

        public PartController(InventoryDbContext context, IPartService partService, ISubassemblyService subassemblyService)
        {
            _context = context;
            _partService = partService;
            _subassemblyService = subassemblyService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all parts", Description = "Retrieves a list of all parts.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<PartResponseDto>))]
        public async Task<ActionResult<IEnumerable<PartResponseDto>>> GetPart()
        {
            return Ok(await _partService.GetAllPartsAsync());
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get part", Description = "Retrieves a part.")]
        [SwaggerResponse(200, "Success", typeof(PartResponseDto))]
        [SwaggerResponse(404, "Part not found")]
        public async Task<ActionResult<PartResponseDto>> GetPart(string id)
        {
            var part = await _partService.GetPartByIdAsync(id);
            if (part == null)
            {
                return NotFound("Part not found");
            }

            return Ok(part);
        }

        [HttpGet("BySubassembly/{id}")]
        [SwaggerOperation(Summary = "Get parts", Description = "Retrieves parts by subassembly Id.")]
        [SwaggerResponse(200, "Success", typeof(PartResponseDto))]
        [SwaggerResponse(404, "Subassembly not found")]
        public async Task<ActionResult<PartResponseDto>> GetPartBySubassembly(string subassemblyId)
        {
            var subassembly = await _subassemblyService.GetSubassemblyByIdAsync(subassemblyId);
            if (subassembly == null)
            {
                return NotFound("Subassembly not found");
            }

            return Ok(await _partService.GetAllPartsBySubassemblyIdAsync(subassemblyId));
        }

        [HttpGet("BySearchString/{searchString}")]
        [SwaggerOperation(Summary = "Get parts containing search string", Description = "Retrieves parts containing search string in WPId, serial number or description.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<PartResponseDto>))]
        public async Task<ActionResult<IEnumerable<PartResponseDto>>> GetPartBySearchString(string searchString)
        {
            return Ok(await _partService.GetAllPartsBySearchStringAsync(searchString));
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new part", Description = "Creates a new part.")]
        [SwaggerResponse(201, "Part created", typeof(PartResponseDto))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<ActionResult<PartResponseDto>> PostPart(PartCreateDto partCreateDto)
        {
            var partId = await _partService.CreatePartAsync(partCreateDto);

            var part = await _partService.GetPartByIdAsync(partId);

            return CreatedAtAction(nameof(GetPart), new { id = partId }, part);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update part", Description = "Updates a part.")]
        [SwaggerResponse(200, "Part updated")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Part not found")]
        public async Task<IActionResult> PutPart(string id, PartUpdateDto partUpdateDto)
        {
            if (id != partUpdateDto.Id)
            {
                return BadRequest("Id does not match");
            }

            var part = await _partService.GetPartByIdAsync(id);
            if (part == null)
            {
                return NotFound("Part not found");
            }

            await _partService.UpdatePartAsync(partUpdateDto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete part", Description = "Deletes a part.")]
        [SwaggerResponse(200, "Part deleted")]
        [SwaggerResponse(404, "Part not found")]
        public async Task<IActionResult> DeletePart(string id)
        {
            var part = _partService.GetPartByIdAsync(id);
            if (part == null)
            {
                return NotFound("Part not found");
            }

            await _partService.DeletePartAsync(id);

            return NoContent();
        }
    }
}
