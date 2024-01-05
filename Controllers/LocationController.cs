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
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }
        
        [HttpGet]
        [SwaggerOperation(Summary = "Get all locations", Description = "Retrieves a list of all locations.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<Location>))]
        public async Task<ActionResult<IEnumerable<Location>>> GetAllLocations()
        {
            return Ok(await _locationService.GetAllLocationsAsync());
        }
        
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get location", Description = "Retrieves a location.")]
        [SwaggerResponse(200, "Success", typeof(Location))]
        [SwaggerResponse(404, "Location not found")]
        public async Task<ActionResult<Location>> GetLocation(string id)
        {
            var location = await _locationService.GetLocationByIdAsync(id);
            if (location == null)
            {
                return NotFound("Location not found");
            }

            return Ok(location);
        }
        
        
        [HttpGet("BySearchString/{searchString}")]
        [SwaggerOperation(Summary = "Get locations containing search string", Description = "Retrieves locations containing search string in name.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<Location>))]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocationsBySearchString(string searchString)
        {
            return Ok(await _locationService.GetAllLocationsBySearchStringAsync(searchString));
        }
        
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new location", Description = "Creates a new location.")]
        [SwaggerResponse(201, "Location created", typeof(Location))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<ActionResult<Location>> CreateLocation(Location locationCreate, [FromServices] IValidator<LocationCreateDto> validator)
        {
            var validationResult = await validator.ValidateAsync(locationCreate);
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
            
            var locationId = await _locationService.CreateLocationAsync(locationCreate);
            if (locationId == null)
            {
                return BadRequest("Location creation failed");
            }

            var location = await _locationService.GetLocationByIdAsync(locationId);

            return CreatedAtAction(nameof(GetLocation), new { id = locationId }, location);
        }
        
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update location", Description = "Updates a location.")]
        [SwaggerResponse(200, "Location updated")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Location not found")]
        public async Task<IActionResult> UpdateLocation(string id, Location locationUpdate, [FromServices] IValidator<LocationUpdateDto> validator)
        {
            var validationResult = await validator.ValidateAsync(locationUpdate);
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
            
            if (id != locationUpdate.Id)
            {
                return BadRequest("Id does not match");
            }

            var location = await _locationService.GetLocationByIdAsync(id);
            if (location == null)
            {
                return NotFound("Location not found");
            }

            await _locationService.UpdateLocationAsync(locationUpdate);

            return NoContent();
        }
        
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete location", Description = "Deletes a location.")]
        [SwaggerResponse(200, "Location deleted")]
        [SwaggerResponse(404, "Location not found")]
        public async Task<IActionResult> DeleteLocation(string id)
        {
            var location = await _locationService.GetLocationByIdAsync(id);
            if (location == null)
            {
                return NotFound("Location not found");
            }

            await _locationService.DeleteLocationAsync(id);

            return NoContent();
        }
    }
}
