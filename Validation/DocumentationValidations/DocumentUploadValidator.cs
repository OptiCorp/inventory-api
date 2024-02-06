using FluentValidation;
using Inventory.Models.DTO;

namespace Inventory.Validations.DocumentValidations;

public class DocumentUploadValidator : AbstractValidator<DocumentUploadDto>, IDocumentUploadValidator
{

    public DocumentUploadValidator()
    {
        RuleFor(document => document.DocumentTypeId).NotEmpty().WithMessage("Document type Id is required.")
            .NotNull().WithMessage("Document type Id cannot be null.");

        RuleFor(document => document.File).NotEmpty().WithMessage("File is required.")
            .NotNull().WithMessage("File cannot be null.");
    }
}