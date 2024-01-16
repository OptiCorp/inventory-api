using Inventory.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Inventory.Services;
using Inventory.Validations.CategoryValidations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ICategoryCreateValidator _createValidator;
        private readonly ICategoryUpdateValidator _updateValidator;

        public CategoryController(ICategoryService categoryService, ICategoryCreateValidator createValidator, ICategoryUpdateValidator updateValidator)
        {
            _categoryService = categoryService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }
        
        [HttpGet]
        [SwaggerOperation(Summary = "Get all categories", Description = "Retrieves a list of all categories.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<Category>))]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            return Ok(await _categoryService.GetAllCategoriesAsync());
        }
        
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get category", Description = "Retrieves a category.")]
        [SwaggerResponse(200, "Success", typeof(Category))]
        [SwaggerResponse(404, "Category not found")]
        public async Task<ActionResult<Category>> GetCategory(string id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound("Category not found");
            }

            return Ok(category);
        }
        
        [HttpGet("BySearchString/{searchString}")]
        [SwaggerOperation(Summary = "Get categories containing search string", Description = "Retrieves categories containing search string in title.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<Category>))]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategoryBySearchString(string searchString)
        {
            return Ok(await _categoryService.GetAllCategoriesBySearchStringAsync(searchString));
        }
        
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new category", Description = "Creates a new category.")]
        [SwaggerResponse(201, "Category created", typeof(Category))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<ActionResult<Category>> CreateCategory(Category categoryCreate)
        {
            var validationResult = await _createValidator.ValidateAsync(categoryCreate);
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
            
            var categoryId = await _categoryService.CreateCategoryAsync(categoryCreate);
            if (categoryId == null)
            {
                return BadRequest("Category creation failed");
            }

            var category = await _categoryService.GetCategoryByIdAsync(categoryId);

            return CreatedAtAction(nameof(GetCategory), new { id = categoryId }, category);
        }
        
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update category", Description = "Updates a category.")]
        [SwaggerResponse(200, "Category updated")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Category not found")]
        public async Task<IActionResult> UpdateCategory(string id, Category categoryUpdate)
        {
            var validationResult = await _updateValidator.ValidateAsync(categoryUpdate);
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
            
            if (id != categoryUpdate.Id)
            {
                return BadRequest("Id does not match");
            }

            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound("Category not found");
            }

            await _categoryService.UpdateCategoryAsync(categoryUpdate);

            return NoContent();
        }
        
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete category", Description = "Deletes a category.")]
        [SwaggerResponse(200, "Category deleted")]
        [SwaggerResponse(404, "Category not found")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound("Category not found");
            }

            await _categoryService.DeleteCategoryAsync(id);

            return NoContent();
        }
    }
}
