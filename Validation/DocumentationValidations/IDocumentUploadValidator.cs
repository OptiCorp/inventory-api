using FluentValidation;
using Inventory.Models.DTO;

namespace Inventory.Validations.DocumentValidations;

public interface IDocumentUploadValidator : IValidator<DocumentUploadDto>;