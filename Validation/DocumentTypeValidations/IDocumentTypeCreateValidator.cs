using FluentValidation;
using Inventory.Models.DTO;

namespace Inventory.Validations.DocumentTypeValidations;

public interface IDocumentTypeCreateValidator : IValidator<DocumentTypeCreateDto>
{
}