using FluentValidation;
using Inventory.Models;

namespace Inventory.Validations.ItemTemplateValidations
{
    public class ItemTemplateUpdateValidator : AbstractValidator<ItemTemplate>, IItemTemplateUpdateValidator
    {

        public ItemTemplateUpdateValidator()
        {
            RuleFor(itemTemplate => itemTemplate.Id).NotEmpty().WithMessage("Item template Id is required.")
                .NotNull().WithMessage("Item template Id cannot be null.");

            RuleFor(itemTemplate => itemTemplate.Type).NotEmpty().WithMessage("Item template type is required.")
                .NotNull().WithMessage("Item template type cannot be null.")
                .MinimumLength(3).WithMessage("Item template type must be at least 3 characters.")
                .MaximumLength(40).WithMessage("Item template type cannot exceed 40 characters.")
                .Matches("^[a-zA-Z0-9_,.:\\- ]+$").WithMessage("Item template type can only contain letters, numbers, underscores, commas, colons, periods or hyphens.");

            RuleFor(itemTemplate => itemTemplate.CategoryId).NotEmpty().WithMessage("Category Id is required.")
                .NotNull().WithMessage("Category Id cannot be null.")
                .MinimumLength(3).WithMessage("Category Id must be at least 3 characters.")
                .MaximumLength(40).WithMessage("Category Id cannot exceed 40 characters.")
                .Matches("^[a-zA-Z0-9_,.:\\- ]+$").WithMessage("Category Id can only contain letters, numbers, underscores, commas, colons, periods or hyphens.");

            RuleFor(itemTemplate => itemTemplate.ProductNumber).NotEmpty().WithMessage("Product number is required.")
                .NotNull().WithMessage("Product number cannot be null.")
                .MinimumLength(3).WithMessage("Product number must be at least 3 characters.")
                .MaximumLength(40).WithMessage("Product number cannot exceed 40 characters.")
                .Matches("^[a-zA-Z0-9_,.:\\- ]+$").WithMessage("Product number can only contain letters, numbers, underscores, commas, colons, periods or hyphens.");

            RuleFor(itemTemplate => itemTemplate.Revision).NotEmpty().WithMessage("Revision is required.")
                .NotNull().WithMessage("Revision cannot be null.")
                .MinimumLength(3).WithMessage("Revision must be at least 3 characters.")
                .MaximumLength(40).WithMessage("Revision cannot exceed 40 characters.")
                .Matches("^[a-zA-Z0-9_,.:\\- ]+$").WithMessage("Revision can only contain letters, numbers, underscores, commas, colons, periods or hyphens.");

            RuleFor(itemTemplate => itemTemplate.CreatedById).NotEmpty().WithMessage("CreatedById is required.")
                .NotNull().WithMessage("CreatedById cannot be null.");

            RuleFor(itemTemplate => itemTemplate.Description).NotEmpty().WithMessage("Description is required.")
                .NotNull().WithMessage("Description cannot be null.")
                .MaximumLength(200).WithMessage("Description cannot exceed 200 characters.");
        }
    }
}