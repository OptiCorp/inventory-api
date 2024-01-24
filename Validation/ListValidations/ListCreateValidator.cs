using FluentValidation;
using Inventory.Models.DTO;

namespace Inventory.Validations.ListValidations
{
    public class ListCreateValidator : AbstractValidator<ListCreateDto>, IListCreateValidator
    {

        public ListCreateValidator()
        {
            RuleFor(list => list.Title).NotEmpty().WithMessage("List title is required.")
                .NotNull().WithMessage("List title cannot be null.")
                .MinimumLength(3).WithMessage("List title must be at least 3 characters.")
                .MaximumLength(40).WithMessage("List title cannot exceed 40 characters.")
                .Matches("^[a-zA-Z0-9_,.:\\- ]+$").WithMessage("List title can only contain letters, numbers, underscores, commas, colons, periods or hyphens.");

            RuleFor(list => list.CreatedById).NotEmpty().WithMessage("CreatedById is required.")
                .NotNull().WithMessage("CreatedById cannot be null.");
        }
    }
}