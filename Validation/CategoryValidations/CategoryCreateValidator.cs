using FluentValidation;
using Inventory.Models.DTOs.CategoryDTOs;

namespace Inventory.Validations.CategoryValidations
{
    public class CategoryCreateValidator : AbstractValidator<CategoryCreateDto>
    {

        public CategoryCreateValidator()
        {
            RuleFor(category => category.Name).NotEmpty().WithMessage("Category name is required.")
                .NotNull().WithMessage("Category name cannot be null.")
                .MinimumLength(3).WithMessage("Category name must be at least 3 characters.")
                .MaximumLength(40).WithMessage("Category name cannot exceed 40 characters.")
                .Matches("^[a-zA-Z0-9_,.:\\- ]+$").WithMessage("Category name can only contain letters, numbers, underscores, commas, colons, periods or hyphens.");

            RuleFor(category => category.AddedById).NotEmpty().WithMessage("AddedById is required.")
                .NotNull().WithMessage("AddedById cannot be null.");
        }
    }
}