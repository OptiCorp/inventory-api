using FluentValidation;
using FluentValidation.Results;
using Inventory.Models;

namespace Inventory.Validations.PreCheckValidations;

public interface IPreCheckUpdateValidator : IValidator<PreCheck>
{
}