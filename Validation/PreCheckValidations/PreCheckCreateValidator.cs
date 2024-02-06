using FluentValidation;
using Inventory.Models.DTO;

namespace Inventory.Validations.PreCheckValidations;

public class PreCheckCreateValidator : AbstractValidator<PreCheckCreateDto>, IPreCheckCreateValidator
{

    public PreCheckCreateValidator()
    {
        RuleFor(preCheck => preCheck.Check).Must(check => check == true).WithMessage("Check has to be true.");

        RuleFor(preCheck => preCheck.Comment)
            .MaximumLength(1000).WithMessage("Comment cannot exceed 1000 characters.");
    }
}