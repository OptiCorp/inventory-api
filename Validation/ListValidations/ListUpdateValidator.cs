using FluentValidation;
using Inventory.Models;

namespace Inventory.Validations.ListValidations
{
    public class ListUpdateValidator : AbstractValidator<List>, IListUpdateValidator
    {

        public ListUpdateValidator()
        {
            RuleFor(list => list.Title).NotEmpty().WithMessage("List title is required.")
                .NotNull().WithMessage("List title cannot be null.")
                .MinimumLength(3).WithMessage("List title must be at least 3 characters.")
                .MaximumLength(40).WithMessage("List title cannot exceed 40 characters.")
                .Matches("^[a-zA-Z0-9_,.:\\- ]+$").WithMessage("List title can only contain letters, numbers, underscores, commas, colons, periods or hyphens.");
            
            RuleFor(list => list.Id).NotEmpty().WithMessage("List Id is required.")
                .NotNull().WithMessage("List Id cannot be null.");
        }
    }
}