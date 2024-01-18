using Inventory.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Inventory.Services;
using Inventory.Validations.LocationValidations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;
        private readonly ILocationCreateValidator _createValidator;
        private readonly ILocationUpdateValidator _updateValidator;

        public LocationController(ILocationService locationService, ILocationCreateValidator createValidator, ILocationUpdateValidator updateValidator)
        {
            _locationService = locationService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }
        
        [HttpGet]
        [SwaggerOperation(Summary = "Get all locations", Description = "Retrieves a list of all locations.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<Location>))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<ActionResult<IEnumerable<Location>>> GetAllLocations()
        {
            try
            {
                return Ok(await _locationService.GetAllLocationsAsync());
            }
            catch (Exception e)
            {
                return BadRequest($"Something went wrong: {e.Message}");
            }
        }
        
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get location", Description = "Retrieves a location.")]
        [SwaggerResponse(200, "Success", typeof(Location))]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Location not found")]
        public async Task<ActionResult<Location>> GetLocation(string id)
        {
            try
            {
                var location = await _locationService.GetLocationByIdAsync(id);
                if (location == null)
                {
                    return NotFound("Location not found");
                }

                return Ok(location);
            }
            catch (Exception e)
            {
                return BadRequest($"Something went wrong: {e.Message}");
            }
        }
        
        
        [HttpGet("BySearchString/{searchString}")]
        [SwaggerOperation(Summary = "Get locations containing search string", Description = "Retrieves locations containing search string in name.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<Location>))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocationsBySearchString(string searchString)
        {
            try
            {
                return Ok(await _locationService.GetAllLocationsBySearchStringAsync(searchString));
            }
            catch (Exception e)
            {
                return BadRequest($"Something went wrong: {e.Message}");
            }
        }
        
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new location", Description = "Creates a new location.")]
        [SwaggerResponse(201, "Location created", typeof(Location))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<ActionResult<Location>> CreateLocation(Location locationCreate)
        {
            var validationResult = await _createValidator.ValidateAsync(locationCreate);
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

            try
            {
                var locationId = await _locationService.CreateLocationAsync(locationCreate);
                if (locationId == null)
                {
                    return BadRequest("Location creation failed");
                }

                var location = await _locationService.GetLocationByIdAsync(locationId);

                return CreatedAtAction(nameof(GetLocation), new { id = locationId }, location);
            }
            catch (Exception e)
            {
                return BadRequest($"Something went wrong: {e.Message}");
            }
        }
        
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update location", Description = "Updates a location.")]
        [SwaggerResponse(200, "Location updated")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Location not found")]
        public async Task<IActionResult> UpdateLocation(string id, Location locationUpdate)
        {
            var validationResult = await _updateValidator.ValidateAsync(locationUpdate);
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

            try
            {
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
            catch (Exception e)
            {
                return BadRequest($"Something went wrong: {e.Message}");
            }
        }
        
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete location", Description = "Deletes a location.")]
        [SwaggerResponse(200, "Location deleted")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Location not found")]
        public async Task<IActionResult> DeleteLocation(string id)
        {
            try
            {
                var location = await _locationService.GetLocationByIdAsync(id);
                if (location == null)
                {
                    return NotFound("Location not found");
                }

                await _locationService.DeleteLocationAsync(id);

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest($"Something went wrong: {e.Message}");
            }
        }
    }
}
