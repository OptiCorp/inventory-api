using FluentValidation;
using Inventory.Models;

namespace Inventory.Validations.PreCheckValidations;

public interface IPreCheckCreateValidator : IValidator<PreCheck>
{
}