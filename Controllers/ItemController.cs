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
    public class ItemController : ControllerBase
    {
        private readonly InventoryDbContext _context;
        private readonly IPartService _partService;
        private readonly IAssemblyService _assemblyService;
        private readonly IUnitService _unitService;
        private readonly ISubassemblyService _subassemblyService;

        public ItemController(InventoryDbContext context, IPartService partService, ISubassemblyService subassemblyService, IUnitService unitService, IAssemblyService assemblyService)
        {
            _context = context;
            _partService = partService;
            _subassemblyService = subassemblyService;
            _assemblyService = assemblyService;
            _unitService = unitService;
        }

        [HttpGet("BySearchString/{searchString}")]
        [SwaggerOperation(Summary = "Get items containing search string", Description = "Retrieves items containing search string in WPId, serial number or description.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<object>))]
        public async Task<ActionResult<List<object>>> GetItemBySearchString(string searchString)
        {
            var items = new List<IEnumerable<object>>();
            items.Add(await _unitService.GetAllUnitsBySearchStringAsync(searchString));
            items.Add(await _assemblyService.GetAllAssembliesBySearchStringAsync(searchString));
            items.Add(await _subassemblyService.GetAllSubassembliesBySearchStringAsync(searchString));
            items.Add(await _partService.GetAllPartsBySearchStringAsync(searchString));

            return Ok(items);
        }
    }
}
