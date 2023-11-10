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
using inventoryapi.Migrations;

namespace Inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubassemblyController : ControllerBase
    {
        private readonly InventoryDbContext _context;

        private readonly ISubassemblyService _subassemblyService;

        public SubassemblyController(InventoryDbContext context, ISubassemblyService subassemblyService)
        {
            _context = context;
            _subassemblyService = subassemblyService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all subassemblies", Description = "Retrieves a list of all subassemblies.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<SubassemblyResponseDto>))]
        public async Task<ActionResult<IEnumerable<SubassemblyResponseDto>>> GetSubassembly()
        {
            return Ok(await _subassemblyService.GetAllSubassembliesAsync());
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get subassembly", Description = "Retrieves a subassembly.")]
        [SwaggerResponse(200, "Success", typeof(SubassemblyResponseDto))]
        [SwaggerResponse(404, "Subassembly not found")]
        public async Task<ActionResult<Equipment>> GetSubassembly(string id)
        {
            var subassembly = await _subassemblyService.GetSubassemblyByIdAsync(id);
            if (subassembly == null)
            {
                return NotFound("Subassembly not found");
            }

            return Ok(subassembly);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new subassembly", Description = "Creates a new subassembly.")]
        [SwaggerResponse(201, "Subassembly created", typeof(SubassemblyResponseDto))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<ActionResult<SubassemblyResponseDto>> PostSubassembly(SubassemblyCreateDto subassemblyCreateDto)
        {
            var subassemblyId = await _subassemblyService.CreateSubassemblyAsync(subassemblyCreateDto);

            var subassembly = await _subassemblyService.GetSubassemblyByIdAsync(subassemblyId);

            return CreatedAtAction(nameof(GetSubassembly), new { id = subassemblyId }, subassembly);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update subassembly", Description = "Updates a subassembly.")]
        [SwaggerResponse(200, "Subassembly updated")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Subassembly not found")]
        public async Task<IActionResult> PutSubassembly(string id, SubassemblyUpdateDto subassemblyUpdateDto)
        {
            if (id != subassemblyUpdateDto.Id)
            {
                return BadRequest("Id does not match");
            }

            var subassembly = await _subassemblyService.GetSubassemblyByIdAsync(id);
            if (subassembly == null)
            {
                return NotFound("Subassembly not found");
            }

            await _subassemblyService.UpdateSubassemblyAsync(subassemblyUpdateDto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete subassembly", Description = "Deletes a subassembly.")]
        [SwaggerResponse(200, "Subassembly deleted")]
        [SwaggerResponse(404, "Subassembly not found")]
        public async Task<IActionResult> DeleteSubassembly(string id)
        {
            var subassembly = _subassemblyService.GetSubassemblyByIdAsync(id);
            if (subassembly == null)
            {
                return NotFound("Subassembly not found");
            }

            await _subassemblyService.DeleteSubassemblyAsync(id);

            return NoContent();
        }
    }
}
