using FluentValidation;
using FluentValidation.Results;
using Inventory.Models;

namespace Inventory.Validations.PreCheckValidations
{
    public class PreCheckCreateValidator : AbstractValidator<PreCheck>, IPreCheckCreateValidator
    {

        public PreCheckCreateValidator()
        {
            RuleFor(preCheck => preCheck.Check).Must(check => check == true).WithMessage("Check has to be true.");
            
            RuleFor(preCheck => preCheck.Comment)
                .MaximumLength(200).WithMessage("Comment cannot exceed 200 characters.");
        }
        
        public async Task<ValidationResult> ValidateAsync(PreCheck preCheck)
        {
            var result = await ValidateAsync(preCheck);
            return result;
        }
    }
}
