using FluentValidation;
using Inventory.Models.DTOs.VendorDTOs;

namespace Inventory.Validations.VendorValidations
{
    public class LocationUpdateValidator : AbstractValidator<VendorUpdateDto>
    {

        public LocationUpdateValidator()
        {
            RuleFor(location => location.Name).NotEmpty().WithMessage("Vendor name is required.")
                .NotNull().WithMessage("Vendor name cannot be null.")
                .MinimumLength(3).WithMessage("Vendor name must be at least 3 characters.")
                .MaximumLength(40).WithMessage("Vendor name cannot exceed 40 characters.")
                .Matches("^[a-zA-Z0-9_.\\- ]+$").WithMessage("Vendor name can only contain letters, numbers, underscores, periods or hyphens.");
            
            RuleFor(location => location.Id).NotEmpty().WithMessage("Location Id is required.")
                .NotNull().WithMessage("Location Id cannot be null.");
        }
    }
}