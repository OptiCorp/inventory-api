using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Inventory.Models.DTOs.CategoryDtos;
using Inventory.Services;

namespace Inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IUserService _userService;

        public CategoryController(ICategoryService categoryService, IUserService userService)
        {
            _categoryService = categoryService;
            _userService = userService;
        }
        
        [HttpGet]
        [SwaggerOperation(Summary = "Get all categories", Description = "Retrieves a list of all categories.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<CategoryResponseDto>))]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetCategory()
        {
            return Ok(await _categoryService.GetAllCategoriesAsync());
        }
        
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get category", Description = "Retrieves a category.")]
        [SwaggerResponse(200, "Success", typeof(CategoryResponseDto))]
        [SwaggerResponse(404, "Category not found")]
        public async Task<ActionResult<CategoryResponseDto>> GetCategory(string id)
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
        [SwaggerResponse(200, "Success", typeof(IEnumerable<CategoryResponseDto>))]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetCategoryBySearchString(string searchString)
        {
            return Ok(await _categoryService.GetAllCategoriesBySearchStringAsync(searchString));
        }
        
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new category", Description = "Creates a new category.")]
        [SwaggerResponse(201, "Category created", typeof(CategoryResponseDto))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<ActionResult<CategoryResponseDto>> PostCategory(CategoryCreateDto categoryCreateDto)
        {
            var categoryId = await _categoryService.CreateCategoryAsync(categoryCreateDto);
            if (categoryId == null)
            {
                return StatusCode(500);
            }

            var category = await _categoryService.GetCategoryByIdAsync(categoryId);

            return CreatedAtAction(nameof(GetCategory), new { id = categoryId }, category);
        }
        
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update category", Description = "Updates a category.")]
        [SwaggerResponse(200, "Category updated")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Category not found")]
        public async Task<IActionResult> PutCategory(string id, CategoryUpdateDto categoryUpdateDto)
        {
            if (id != categoryUpdateDto.Id)
            {
                return BadRequest("Id does not match");
            }

            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound("Category not found");
            }

            await _categoryService.UpdateCategoryAsync(categoryUpdateDto);

            return NoContent();
        }
        
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete category", Description = "Deletes an category.")]
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
