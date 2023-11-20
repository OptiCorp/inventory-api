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
        private readonly IItemService _itemService;
        private readonly IAssemblyService _assemblyService;
        private readonly IUnitService _unitService;
        private readonly ISubassemblyService _subassemblyService;

        public PartController(InventoryDbContext context, IItemService itemService, ISubassemblyService subassemblyService, IUnitService unitService, IAssemblyService assemblyService)
        {
            _context = context;
            _itemService = itemService;
            _subassemblyService = subassemblyService;
            _assemblyService = assemblyService;
            _unitService = unitService;
        }

        [HttpGet("BySearchString/{searchString}")]
        [SwaggerOperation(Summary = "Get parts containing search string", Description = "Retrieves parts containing search string in WPId, serial number or description.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<object>))]
        public async Task<ActionResult<List<object>>> GetPartBySearchString(string searchString)
        {
            var parts = new List<IEnumerable<object>>();
            parts.Add(await _unitService.GetAllUnitsBySearchStringAsync(searchString));
            parts.Add(await _assemblyService.GetAllAssembliesBySearchStringAsync(searchString));
            parts.Add(await _subassemblyService.GetAllSubassembliesBySearchStringAsync(searchString));
            parts.Add(await _itemService.GetAllItemsBySearchStringAsync(searchString));

            return Ok(parts);
        }
    }
}
