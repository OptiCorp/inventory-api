using FluentValidation;
using Inventory.Models.DTO;

namespace Inventory.Validations.LocationValidations
{
    public class LocationCreateValidator : AbstractValidator<LocationCreateDto>, ILocationCreateValidator
    {

        public LocationCreateValidator()
        {
            RuleFor(location => location.Name).NotEmpty().WithMessage("Location name is required.")
                .NotNull().WithMessage("Location name cannot be null.")
                .MinimumLength(3).WithMessage("Location name must be at least 3 characters.")
                .MaximumLength(40).WithMessage("Location name cannot exceed 40 characters.")
                .Matches("^[a-zA-Z0-9_,.:\\- ]+$").WithMessage("Location name can only contain letters, numbers, underscores, commas, colons, periods or hyphens.");

            RuleFor(location => location.CreatedById).NotEmpty().WithMessage("AddedById is required.")
                .NotNull().WithMessage("AddedById cannot be null.");
        }
    }
}