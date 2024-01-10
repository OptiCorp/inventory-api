using FluentValidation;
using FluentValidation.Results;
using Inventory.Models;

namespace Inventory.Validations.PreCheckValidations
{
    public class PreCheckUpdateValidator : AbstractValidator<PreCheck>, IPreCheckUpdateValidator
    {

        public PreCheckUpdateValidator()
        {
            RuleFor(preCheck => preCheck.Check).Must(check => check == true).WithMessage("Check has to be true.");
            
            RuleFor(preCheck => preCheck.Comment)
                .MaximumLength(200).WithMessage("Comment cannot exceed 200 characters.");
            
            RuleFor(preCheck => preCheck.Id).NotEmpty().WithMessage("PreCheck Id is required.")
                .NotNull().WithMessage("PreCheck Id cannot be null.");
        }
    }
}
