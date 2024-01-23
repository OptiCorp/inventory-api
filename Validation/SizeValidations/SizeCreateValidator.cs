using FluentValidation;
using Inventory.Models.DTO;

namespace Inventory.Validations.SizeValidations
{
    public class SizeCreateValidator : AbstractValidator<SizeCreateDto>, ISizeCreateValidator
    {

        public SizeCreateValidator()
        {
            RuleFor(size => size.ItemTemplateId).NotEmpty().WithMessage("Item template Id is required.")
                .NotNull().WithMessage("Item template Id cannot be null.");

            RuleFor(size => size.Property)
                .MinimumLength(3).WithMessage("Property must be at least 3 characters.")
                .MaximumLength(40).WithMessage("Property cannot exceed 40 characters.")
                .Matches("^[a-zA-Z0-9_,.:\\- ]+$").WithMessage("Property can only contain letters, numbers, underscores, commas, colons, periods or hyphens.");

            RuleFor(size => size.Amount)
                .Must(value => float.TryParse(value.ToString(), out _))
                .WithMessage("Amount must be a number.");

            RuleFor(size => size.Unit)
                .MinimumLength(1).WithMessage("Unit must be at least 1 character.")
                .MaximumLength(40).WithMessage("Unit cannot exceed 40 characters.");
        }
    }
}