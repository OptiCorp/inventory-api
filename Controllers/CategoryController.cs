using Inventory.Models;
using Inventory.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Inventory.Services;
using Inventory.Validations.CategoryValidations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authorization;

namespace Inventory.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController(
    ICategoryService categoryService,
    ICategoryCreateValidator createValidator,
    ICategoryUpdateValidator updateValidator)
    : ControllerBase
{
    // [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Policy = "AuthZPolicy")] 

    [HttpGet]
    [SwaggerOperation(Summary = "Get all categories", Description = "Retrieves a list of all categories.")]
    [SwaggerResponse(200, "Success", typeof(IEnumerable<Category>))]
    [SwaggerResponse(400, "Invalid request")]
    public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
    {
        try
        {
            return Ok(await categoryService.GetAllCategoriesAsync());
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get category", Description = "Retrieves a category.")]
    [SwaggerResponse(200, "Success", typeof(Category))]
    [SwaggerResponse(400, "Invalid request")]
    [SwaggerResponse(404, "Category not found")]
    public async Task<ActionResult<Category>> GetCategory(string id)
    {
        try
        {
            var category = await categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound("Category not found");
            }
            return Ok(category);
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpGet("BySearchString/{searchString}")]
    [SwaggerOperation(Summary = "Get categories containing search string", Description = "Retrieves categories containing search string in title.")]
    [SwaggerResponse(200, "Success", typeof(IEnumerable<Category>))]
    [SwaggerResponse(400, "Invalid request")]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategoryBySearchString(string searchString)
    {
        try
        {
            return Ok(await categoryService.GetAllCategoriesBySearchStringAsync(searchString));
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create a new category", Description = "Creates a new category.")]
    [SwaggerResponse(201, "Category created", typeof(Category))]
    [SwaggerResponse(400, "Invalid request")]
    public async Task<ActionResult<Category>> CreateCategory(CategoryCreateDto categoryCreate)
    {
        var validationResult = await createValidator.ValidateAsync(categoryCreate);
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
            var categoryId = await categoryService.CreateCategoryAsync(categoryCreate);
            var category = await categoryService.GetCategoryByIdAsync(categoryId!);

            return CreatedAtAction(nameof(GetCategory), new { id = categoryId }, category);
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update category", Description = "Updates a category.")]
    [SwaggerResponse(200, "Category updated")]
    [SwaggerResponse(400, "Invalid request")]
    [SwaggerResponse(404, "Category not found")]
    public async Task<IActionResult> UpdateCategory(string id, Category categoryUpdate)
    {
        var validationResult = await updateValidator.ValidateAsync(categoryUpdate);
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

        try
        {
            var category = await categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound("Category not found");
            }

            await categoryService.UpdateCategoryAsync(categoryUpdate);

            return NoContent();

        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }

    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete category", Description = "Deletes a category.")]
    [SwaggerResponse(200, "Category deleted")]
    [SwaggerResponse(400, "Invalid request")]
    [SwaggerResponse(404, "Category not found")]
    public async Task<IActionResult> DeleteCategory(string id)
    {
        try
        {
            var category = await categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound("Category not found");
            }

            await categoryService.DeleteCategoryAsync(id);

            return NoContent();

        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }
}