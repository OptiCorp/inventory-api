using FluentValidation;
using FluentValidation.Results;
using Inventory.Models;

namespace Inventory.Validations.ItemTemplateValidations
{
    public class ItemTemplateUpdateValidator : AbstractValidator<ItemTemplate>, IItemTemplateUpdateValidator
    {

        public ItemTemplateUpdateValidator()
        {
            RuleFor(itemTemplate => itemTemplate.Id).NotEmpty().WithMessage("Item template Id is required.")
                .NotNull().WithMessage("Item template Id cannot be null.");
            
            RuleFor(itemTemplate => itemTemplate.Name).NotEmpty().WithMessage("Item template name is required.")
                .NotNull().WithMessage("Item template name cannot be null.")
                .MinimumLength(3).WithMessage("Item template name must be at least 3 characters.")
                .MaximumLength(40).WithMessage("Item template name cannot exceed 40 characters.")
                .Matches("^[a-zA-Z0-9_,.:\\- ]+$").WithMessage("Item template name can only contain letters, numbers, underscores, commas, colons, periods or hyphens.");
            
            RuleFor(itemTemplate => itemTemplate.Type).NotEmpty().WithMessage("Item template type is required.")
                .NotNull().WithMessage("Item template type cannot be null.")
                .MinimumLength(3).WithMessage("Item template type must be at least 3 characters.")
                .MaximumLength(40).WithMessage("Item template type cannot exceed 40 characters.")
                .Matches("^[a-zA-Z0-9_,.:\\- ]+$").WithMessage("Item template type can only contain letters, numbers, underscores, commas, colons, periods or hyphens.");

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

            RuleFor(itemTemplate => itemTemplate.Description)
                .MaximumLength(200).WithMessage("Description cannot exceed 200 characters.");
        }
    }
}