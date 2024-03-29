using FluentValidation;
using Inventory.Models;

namespace Inventory.Validations.PreCheckValidations;

public class PreCheckUpdateValidator : AbstractValidator<PreCheck>, IPreCheckUpdateValidator
{

    public PreCheckUpdateValidator()
    {
        RuleFor(preCheck => preCheck.Check).Must(check => check == true).WithMessage("Check has to be true.");

        RuleFor(preCheck => preCheck.Comment)
            .MaximumLength(1000).WithMessage("Comment cannot exceed 1000 characters.");

        RuleFor(preCheck => preCheck.Id).NotEmpty().WithMessage("PreCheck Id is required.")
            .NotNull().WithMessage("PreCheck Id cannot be null.");
    }
}