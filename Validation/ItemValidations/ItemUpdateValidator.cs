using FluentValidation;
using Inventory.Models;

namespace Inventory.Validations.ItemValidations;

public class ItemUpdateValidator : AbstractValidator<Item>, IItemUpdateValidator
{

    public ItemUpdateValidator()
    {
        RuleFor(item => item.Id).NotEmpty().WithMessage("Item Id is required.")
            .NotNull().WithMessage("Item Id cannot be null.");

        RuleFor(item => item.WpId).NotEmpty().WithMessage("WellPartner Id is required.")
            .NotNull().WithMessage("WellPartner Id cannot be null.")
            .MinimumLength(3).WithMessage("WellPartner Id must be at least 3 characters.")
            .MaximumLength(40).WithMessage("WellPartner Id cannot exceed 40 characters.")
            .Matches("^[a-zA-Z0-9_,.:\\- ]+$").WithMessage("WellPartner Id can only contain letters, numbers, underscores, commas, colons, periods or hyphens.");

        RuleFor(item => item.ItemTemplateId).NotEmpty().WithMessage("Item template ID is required.")
            .NotNull().WithMessage("Item template ID cannot be null")
            .MinimumLength(3).WithMessage("Item template must be at least 3 characters.")
            .MaximumLength(40).WithMessage("Item template ID cannot exceed 40 characters.")
            .Matches("^[a-zA-Z0-9_,.:\\- ]+$").WithMessage("Serial number can only contain letters, numbers, underscores, commas, colons, periods or hyphens.");

        RuleFor(item => item.SerialNumber).NotEmpty().WithMessage("Serial number is required.")
            .NotNull().WithMessage("Serial number cannot be null.")
            .MinimumLength(3).WithMessage("Serial number must be at least 3 characters.")
            .MaximumLength(40).WithMessage("Serial number cannot exceed 40 characters.")
            .Matches("^[a-zA-Z0-9_,.:\\- ]+$").WithMessage("Serial number can only contain letters, numbers, underscores, commas, colons, periods or hyphens.");

        RuleFor(item => item.ParentId)
            .MinimumLength(3).WithMessage("Parent Id must be at least 3 characters.")
            .MaximumLength(40).WithMessage("Parent Id cannot exceed 40 characters.")
            .Matches("^[a-zA-Z0-9_,.:\\- ]+$").WithMessage("Parent Id can only contain letters, numbers, underscores, commas, colons, periods or hyphens.");

        RuleFor(item => item.VendorId).NotEmpty().WithMessage("Vendor Id is required.")
            .NotNull().WithMessage("Vendor Id cannot be null.")
            .MinimumLength(3).WithMessage("Vendor Id must be at least 3 characters.")
            .MaximumLength(40).WithMessage("Vendor Id cannot exceed 40 characters.")
            .Matches("^[a-zA-Z0-9_,.:\\- ]+$").WithMessage("Vendor Id can only contain letters, numbers, underscores, commas, colons, periods or hyphens.");

        RuleFor(item => item.LocationId)
            .MinimumLength(3).WithMessage("Location Id must be at least 3 characters.")
            .MaximumLength(40).WithMessage("Location Id cannot exceed 40 characters.")
            .Matches("^[a-zA-Z0-9_,.:\\- ]+$").WithMessage("Location Id can only contain letters, numbers, underscores, commas, colons, periods or hyphens.");


        RuleFor(item => item.CreatedById).NotEmpty().WithMessage("AddedById is required.")
            .NotNull().WithMessage("AddedById cannot be null.");

        RuleFor(item => item.Comment)
            .MaximumLength(1000).WithMessage("Comment cannot exceed 1000 characters.");
    }
}