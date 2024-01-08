using FluentValidation;
using FluentValidation.Results;
using Inventory.Models;

namespace Inventory.Validations.VendorValidations
{
    public class VendorUpdateValidator : AbstractValidator<Vendor>, IVendorUpdateValidator
    {

        public VendorUpdateValidator()
        {
            RuleFor(vendor => vendor.Name).NotEmpty().WithMessage("Vendor name is required.")
                .NotNull().WithMessage("Vendor name cannot be null.")
                .MinimumLength(3).WithMessage("Vendor name must be at least 3 characters.")
                .MaximumLength(40).WithMessage("Vendor name cannot exceed 40 characters.")
                .Matches("^[a-zA-Z0-9_,.:\\- ]+$").WithMessage("Vendor name can only contain letters, numbers, underscores, commas, colons, periods or hyphens.");
            
            RuleFor(vendor => vendor.Id).NotEmpty().WithMessage("Vendor Id is required.")
                .NotNull().WithMessage("Vendor Id cannot be null.");
        }
        
        public async Task<ValidationResult> ValidateAsync(Vendor vendor)
        {
            var result = await ValidateAsync(vendor);
            return result;
        }
    }
}