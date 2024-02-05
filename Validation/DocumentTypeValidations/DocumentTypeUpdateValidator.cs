using FluentValidation;
using Inventory.Models;

namespace Inventory.Validations.DocumentTypeValidations
{
    public class DocumentTypeUpdateValidator : AbstractValidator<DocumentType>, IDocumentTypeUpdateValidator
    {

        public DocumentTypeUpdateValidator()
        {
            RuleFor(documentType => documentType.Name).NotEmpty().WithMessage("Document type name is required.")
                .NotNull().WithMessage("Document type name cannot be null.")
                .MinimumLength(3).WithMessage("Document type name must be at least 3 characters.")
                .MaximumLength(40).WithMessage("Document type name cannot exceed 40 characters.")
                .Matches("^[a-zA-Z0-9_,.:\\- ]+$").WithMessage("Document type name can only contain letters, numbers, underscores, commas, colons, periods or hyphens.");

            RuleFor(documentType => documentType.Description).NotEmpty().WithMessage("Description is required.")
                .NotNull().WithMessage("Description cannot be null.")
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

            RuleFor(documentType => documentType.Id).NotEmpty().WithMessage("Document type Id is required.")
                .NotNull().WithMessage("Document type Id cannot be null.");
        }
    }
}