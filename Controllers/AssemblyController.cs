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
    public class AssemblyController : ControllerBase
    {
        private readonly InventoryDbContext _context;

        private readonly IAssemblyService _assemblyService;

        private readonly IUnitService _unitService;

        public AssemblyController(InventoryDbContext context, IAssemblyService assemblyService, IUnitService unitService)
        {
            _context = context;
            _assemblyService = assemblyService;
            _unitService = unitService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all assemblies", Description = "Retrieves a list of all assemblies.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<AssemblyResponseDto>))]
        public async Task<ActionResult<IEnumerable<AssemblyResponseDto>>> GetAssembly()
        {
            return Ok(await _assemblyService.GetAllAssembliesAsync());
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get assembly", Description = "Retrieves an assembly.")]
        [SwaggerResponse(200, "Success", typeof(AssemblyResponseDto))]
        [SwaggerResponse(404, "Assembly not found")]
        public async Task<ActionResult<AssemblyResponseDto>> GetAssembly(string id)
        {
            var assembly = await _assemblyService.GetAssemblyByIdAsync(id);
            if (assembly == null)
            {
                return NotFound("Assembly not found");
            }

            return Ok(assembly);
        }

        [HttpGet("ByUnit/{id}")]
        [SwaggerOperation(Summary = "Get assemblies", Description = "Retrieves assemblies by unit Id.")]
        [SwaggerResponse(200, "Success", typeof(AssemblyResponseDto))]
        [SwaggerResponse(404, "Unit not found")]
        public async Task<ActionResult<AssemblyResponseDto>> GetAssemblyByUnit(string unitId)
        {
            var unit = await _unitService.GetUnitByIdAsync(unitId);
            if (unit == null)
            {
                return NotFound("Unit not found");
            }

            return Ok(await _assemblyService.GetAllAssembliesByUnitIdAsync(unitId));
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new assembly", Description = "Creates a new assembly.")]
        [SwaggerResponse(201, "Assembly created", typeof(AssemblyResponseDto))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<ActionResult<AssemblyResponseDto>> PostAssembly(AssemblyCreateDto assemblyCreateDto)
        {
            var assemblyId = await _assemblyService.CreateAssemblyAsync(assemblyCreateDto);

            var assembly = await _assemblyService.GetAssemblyByIdAsync(assemblyId);

            return CreatedAtAction(nameof(GetAssembly), new { id = assemblyId }, assembly);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update assembly", Description = "Updates an assembly.")]
        [SwaggerResponse(200, "Assembly updated")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Assembly not found")]
        public async Task<IActionResult> PutAssembly(string id, AssemblyUpdateDto assemblyUpdateDto)
        {
            if (id != assemblyUpdateDto.Id)
            {
                return BadRequest("Id does not match");
            }

            var assembly = await _assemblyService.GetAssemblyByIdAsync(id);
            if (assembly == null)
            {
                return NotFound("Assembly not found");
            }

            await _assemblyService.UpdateAssemblyAsync(assemblyUpdateDto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete assembly", Description = "Deletes an assembly.")]
        [SwaggerResponse(200, "Assembly deleted")]
        [SwaggerResponse(404, "Assembly not found")]
        public async Task<IActionResult> DeleteAssembly(string id)
        {
            var assembly = _assemblyService.GetAssemblyByIdAsync(id);
            if (assembly == null)
            {
                return NotFound("Assembly not found");
            }

            await _assemblyService.DeleteAssemblyAsync(id);

            return NoContent();
        }
    }
}
