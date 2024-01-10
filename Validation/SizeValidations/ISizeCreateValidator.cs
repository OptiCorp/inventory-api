using FluentValidation;
using FluentValidation.Results;
using Inventory.Models;

namespace Inventory.Validations.SizeValidations;

public interface ISizeCreateValidator : IValidator<Size>
{
}