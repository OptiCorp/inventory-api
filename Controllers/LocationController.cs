using FluentValidation;
using Inventory.Models.DTOs.CategoryDTOs;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Inventory.Models.DTOs.LocationDTOs;
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
        [SwaggerResponse(200, "Success", typeof(IEnumerable<LocationResponseDto>))]
        public async Task<ActionResult<IEnumerable<LocationResponseDto>>> GetLocation()
        {
            return Ok(await _locationService.GetAllLocationsAsync());
        }
        
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get location", Description = "Retrieves a location.")]
        [SwaggerResponse(200, "Success", typeof(LocationResponseDto))]
        [SwaggerResponse(404, "Location not found")]
        public async Task<ActionResult<LocationResponseDto>> GetLocation(string id)
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
        [SwaggerResponse(200, "Success", typeof(IEnumerable<LocationResponseDto>))]
        public async Task<ActionResult<IEnumerable<LocationResponseDto>>> GetLocationBySearchString(string searchString)
        {
            return Ok(await _locationService.GetAllLocationsBySearchStringAsync(searchString));
        }
        
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new location", Description = "Creates a new location.")]
        [SwaggerResponse(201, "Location created", typeof(LocationResponseDto))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<ActionResult<LocationResponseDto>> PostLocation(LocationCreateDto locationCreateDto, [FromServices] IValidator<LocationCreateDto> validator)
        {
            var validationResult = await validator.ValidateAsync(locationCreateDto);
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
            
            var locationId = await _locationService.CreateLocationAsync(locationCreateDto);
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
        public async Task<IActionResult> PutLocation(string id, LocationUpdateDto locationUpdateDto, [FromServices] IValidator<LocationUpdateDto> validator)
        {
            var validationResult = await validator.ValidateAsync(locationUpdateDto);
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
            
            if (id != locationUpdateDto.Id)
            {
                return BadRequest("Id does not match");
            }

            var location = await _locationService.GetLocationByIdAsync(id);
            if (location == null)
            {
                return NotFound("Location not found");
            }

            await _locationService.UpdateLocationAsync(locationUpdateDto);

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
