using FluentValidation;
using Inventory.Models;

namespace Inventory.Validations.VendorValidations
{
    public class VendorCreateValidator : AbstractValidator<Vendor>, IVendorCreateValidator
    {

        public VendorCreateValidator()
        {
            RuleFor(vendor => vendor.Name).NotEmpty().WithMessage("Vendor name is required.")
                .NotNull().WithMessage("Vendor name cannot be null.")
                .MinimumLength(3).WithMessage("Vendor name must be at least 3 characters.")
                .MaximumLength(40).WithMessage("Vendor name cannot exceed 40 characters.")
                .Matches("^[a-zA-Z0-9_,.:\\- ]+$").WithMessage("Vendor name can only contain letters, numbers, underscores, commas, colons, periods or hyphens.");
            
            RuleFor(vendor => vendor.CreatedById).NotEmpty().WithMessage("AddedById is required.")
                .NotNull().WithMessage("AddedById cannot be null.");
        }
    }
}