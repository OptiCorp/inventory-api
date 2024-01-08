using FluentValidation;
using FluentValidation.Results;
using Inventory.Models;

namespace Inventory.Validations.LocationValidations
{
    public class LocationUpdateValidator : AbstractValidator<Location>, ILocationUpdateValidator
    {

        public LocationUpdateValidator()
        {
            RuleFor(location => location.Name).NotEmpty().WithMessage("Location name is required.")
                .NotNull().WithMessage("Location name cannot be null.")
                .MinimumLength(3).WithMessage("Location name must be at least 3 characters.")
                .MaximumLength(40).WithMessage("Location name cannot exceed 40 characters.")
                .Matches("^[a-zA-Z0-9_,.:\\- ]+$").WithMessage("Location name can only contain letters, numbers, underscores, commas, colons, periods or hyphens.");
            
            RuleFor(location => location.Id).NotEmpty().WithMessage("Location Id is required.")
                .NotNull().WithMessage("Location Id cannot be null.");
        }
        
        public async Task<ValidationResult> ValidateAsync(Location location)
        {
            var result = await ValidateAsync(location);
            return result;
        }
    }
}