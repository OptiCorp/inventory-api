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
    public class UnitController : ControllerBase
    {
        private readonly InventoryDbContext _context;

        private readonly IUnitService _unitService;

        public UnitController(InventoryDbContext context, IUnitService unitService)
        {
            _context = context;
            _unitService = unitService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all units", Description = "Retrieves a list of all units.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<UnitResponseDto>))]
        public async Task<ActionResult<IEnumerable<UnitResponseDto>>> GetUnit()
        {
            return Ok(await _unitService.GetAllUnitsAsync());
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get unit", Description = "Retrieves a unit.")]
        [SwaggerResponse(200, "Success", typeof(UnitResponseDto))]
        [SwaggerResponse(404, "Unit not found")]
        public async Task<ActionResult<UnitResponseDto>> GetUnit(string id)
        {
            var unit = await _unitService.GetUnitByIdAsync(id);
            if (unit == null)
            {
                return NotFound("Unit not found");
            }

            return Ok(unit);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new unit", Description = "Creates a new unit.")]
        [SwaggerResponse(201, "Unit created", typeof(UnitResponseDto))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<ActionResult<UnitResponseDto>> PostUnit(UnitCreateDto unitCreateDto)
        {
            var unitId = await _unitService.CreateUnitAsync(unitCreateDto);

            var unit = await _unitService.GetUnitByIdAsync(unitId);

            return CreatedAtAction(nameof(GetUnit), new { id = unitId }, unit);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update unit", Description = "Updates a unit.")]
        [SwaggerResponse(200, "Unit updated")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Unit not found")]
        public async Task<IActionResult> PutUnit(string id, UnitUpdateDto unitUpdateDto)
        {
            if (id != unitUpdateDto.Id)
            {
                return BadRequest("Id does not match");
            }

            var unit = await _unitService.GetUnitByIdAsync(id);
            if (unit == null)
            {
                return NotFound("Unit not found");
            }

            await _unitService.UpdateUnitAsync(unitUpdateDto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete unit", Description = "Deletes a unit.")]
        [SwaggerResponse(200, "Unit deleted")]
        [SwaggerResponse(404, "Unit not found")]
        public async Task<IActionResult> DeleteUnit(string id)
        {
            var unit = _unitService.GetUnitByIdAsync(id);
            if (unit == null)
            {
                return NotFound("Unit not found");
            }

            await _unitService.DeleteUnitAsync(id);

            return NoContent();
        }
    }
}
