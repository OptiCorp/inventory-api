using FluentValidation;
using Inventory.Models;

namespace Inventory.Validations.DocumentTypeValidations;

public interface IDocumentTypeCreateValidator : IValidator<DocumentType>
{
}