using Inventory.Models;
using Inventory.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Inventory.Services;
using Inventory.Validations.VendorValidations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly IVendorService _vendorService;
        private readonly IVendorCreateValidator _createValidator;
        private readonly IVendorUpdateValidator _updateValidator;

        public VendorController(IVendorService vendorService, IVendorCreateValidator createValidator, IVendorUpdateValidator updateValidator)
        {
            _vendorService = vendorService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }
        
        [HttpGet]
        [SwaggerOperation(Summary = "Get all vendors", Description = "Retrieves a list of all vendors.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<Vendor>))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<ActionResult<IEnumerable<Vendor>>> GetAllVendors()
        {
            try
            {
                return Ok(await _vendorService.GetAllVendorsAsync());
            }
            catch (Exception e)
            {
                return BadRequest($"Something went wrong: {e.Message}");
            }
        }
        
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get vendor", Description = "Retrieves a vendor.")]
        [SwaggerResponse(200, "Success", typeof(Vendor))]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Vendor not found")]
        public async Task<ActionResult<Vendor>> GetVendor(string id)
        {
            try
            {
                var vendor = await _vendorService.GetVendorByIdAsync(id);
                if (vendor == null)
                {
                    return NotFound("Vendor not found");
                }

                return Ok(vendor);
            }
            catch (Exception e)
            {
                return BadRequest($"Something went wrong: {e.Message}");
            }
        }
        
        [HttpGet("BySearchString/{searchString}")]
        [SwaggerOperation(Summary = "Get vendors containing search string", Description = "Retrieves vendors containing search string in name.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<Vendor>))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<ActionResult<IEnumerable<Vendor>>> GetVendorBySearchString(string searchString)
        {
            try
            { 
                return Ok(await _vendorService.GetAllVendorsBySearchStringAsync(searchString));
            }
            catch (Exception e)
            {
                return BadRequest($"Something went wrong: {e.Message}");
            }
        }
        
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new vendor", Description = "Creates a new vendor.")]
        [SwaggerResponse(201, "Vendor created", typeof(Vendor))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<ActionResult<Vendor>> CreateVendor(VendorCreateDto vendorCreate)
        {
            var validationResult = await _createValidator.ValidateAsync(vendorCreate);
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
                var vendorId = await _vendorService.CreateVendorAsync(vendorCreate);
                if (vendorId == null)
                {
                    return BadRequest("Vendor creation failed");
                }

                var vendor = await _vendorService.GetVendorByIdAsync(vendorId);

                return CreatedAtAction(nameof(GetVendor), new { id = vendorId }, vendor);
            }
            catch (Exception e)
            {
                return BadRequest($"Something went wrong: {e.Message}");
            }
        }
        
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update vendor", Description = "Updates a vendor.")]
        [SwaggerResponse(200, "Vendor updated")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Vendor not found")]
        public async Task<IActionResult> UpdateVendor(string id, Vendor vendorUpdate)
        {
            var validationResult = await _updateValidator.ValidateAsync(vendorUpdate);
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
                if (id != vendorUpdate.Id)
                {
                    return BadRequest("Id does not match");
                }

                var vendor = await _vendorService.GetVendorByIdAsync(id);
                if (vendor == null)
                {
                    return NotFound("Vendor not found");
                }

                await _vendorService.UpdateVendorAsync(vendorUpdate);

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest($"Something went wrong: {e.Message}");
            }
        }
        
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete vendor", Description = "Deletes a vendor.")]
        [SwaggerResponse(200, "Vendor deleted")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Vendor not found")]
        public async Task<IActionResult> DeleteVendor(string id)
        {
            try
            {
                var vendor = await _vendorService.GetVendorByIdAsync(id);
                if (vendor == null)
                {
                    return NotFound("Vendor not found");
                }

                await _vendorService.DeleteVendorAsync(id);

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest($"Something went wrong: {e.Message}");
            }
        }
    }
}
